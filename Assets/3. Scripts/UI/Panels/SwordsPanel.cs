using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Localization;
using _3._Scripts.Pool;
using _3._Scripts.Saves.Handlers;
using _3._Scripts.UI.Elements.SwordsPanel;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public class SwordsPanel : UIPanel
    {
        [Tab("Main")] [SerializeField] private ScaleTransition transition;
        [SerializeField] private SwordItem currentSword;

        [SerializeField] private Transform container;
        [SerializeField] private TMP_Text capacityText;

        [Tab("Selected Buttons")] [SerializeField]
        private Button equipSelectedButton;

        [SerializeField] private Button craftSelectedButton;
        [SerializeField] private LocalizeStringEvent craftingText;

        [Tab("Control Buttons")] [SerializeField]
        private Transform deleteControlsButtonContainer;

        [SerializeField] private Button deleteButton;
        [SerializeField] private Button cancelDeleteButton;
        [SerializeField] private Button acceptDeleteButton;
        [Space] [SerializeField] private Button equipBestButton;
        [SerializeField] private Button craftAllButton;

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }

        private readonly List<SwordItem> _items = new();
        private SwordsSave Save => GBGames.saves.swordsSave;

        private bool _deleteMode;
        private SwordItem _selectedItem;

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;

            deleteButton.onClick.AddListener(() => SetDeletingState(true));
            cancelDeleteButton.onClick.AddListener(() => SetDeletingState(false));
            acceptDeleteButton.onClick.AddListener(DeleteSelected);
            equipBestButton.onClick.AddListener(() => EquipItem(_items.OrderByDescending(i => i.SwordDamage).First()));
            equipSelectedButton.onClick.AddListener(() => EquipItem(_selectedItem));
            craftSelectedButton.onClick.AddListener(MergeSelected);
            craftAllButton.onClick.AddListener(MergeAll);
        }

        protected override void OnOpen()
        {
            currentSword.Initialize(Save.current);

            SetDeletingState(false);
            UpdateCapacityText();
            InitializeList();
            SortObjects();
            UpdateCombineText();

            base.OnOpen();
        }

        private void InitializeList()
        {

            foreach (var save in Save.unlocked)
            {
                var item = ObjectsPoolManager.Instance.Get<SwordItem>();

                item.Initialize(save);
                item.transform.SetParent(container);
                item.transform.localScale = Vector3.one;
                item.OnSelect += ItemOnClick;

                if (item.Save.uid == Save.current.uid)
                {
                    _selectedItem = item;
                    item.SetCurrentFocus();
                }

                _items.Add(item);
            }
        }

        private void ItemOnClick(SwordItem obj)
        {
            if (!_deleteMode)
            {
                currentSword.Initialize(obj.Save);
                if (_selectedItem != null)
                {
                    if (_selectedItem.Save.uid == Save.current.uid)
                        _selectedItem.SetCurrentFocus();
                    else
                        _selectedItem.DisableFocus();
                }

                _selectedItem = obj;
                _selectedItem.SetSelectedFocus();
                UpdateCombineText();
            }
            else
            {
                if (obj.Save.uid != Save.current.uid)
                    obj.SetDeleteState(!obj.ItemToDelete);
            }
        }

        private void SetDeletingState(bool state)
        {
            _deleteMode = state;
            deleteButton.gameObject.SetActive(!state);
            deleteControlsButtonContainer.gameObject.SetActive(state);

            foreach (var item in _items.Where(i => i.Save.uid != Save.current.uid))
            {
                item.DisableFocus();
                if (!state)
                    item.SetDeleteState(false);
            }
        }

        private void DeleteSelected()
        {
            var itemsToDelete = _items.Where(i => i.ItemToDelete).ToList();

            foreach (var item in itemsToDelete)
            {
                Save.Delete(item.Save);
                _items.Remove(item);
            }

            SetDeletingState(false);
        }

        private void SortObjects()
        {
            var sortedObjects = _items
                .OrderByDescending(obj => obj.SwordDamage)
                .ThenByDescending(obj => obj.Config.Rarity)
                .ToArray();

            for (int i = 0; i < sortedObjects.Length; i++)
            {
                sortedObjects[i].transform.SetSiblingIndex(i);
            }
        }

        private void EquipItem(SwordItem swordItem)
        {
            if (swordItem == null) return;

            Save.SetCurrent(swordItem.Save);
            currentSword.Initialize(swordItem.Save);

            foreach (var item in _items)
            {
                item.DisableFocus();
            }

            swordItem.SetCurrentFocus();
        }

        private void MergeSelected()
        {
            Save.TryMergeObject(_selectedItem.Save);
            currentSword.UpdateItem();
            _selectedItem.UpdateItem();
            UpdateCapacityText();
            UpdateCombineText();
            SortObjects();
        }

        private void MergeAll()
        {
            Save.MergeAll();
            currentSword.UpdateItem();
            ClearItems();
            UpdateCapacityText();
            InitializeList();
            SortObjects();
            UpdateCombineText();
        }

        protected override void OnClose()
        {
            base.OnClose();
            ClearItems();
        }

        private void ClearItems()
        {
            foreach (var item in _items)
            {
                ObjectsPoolManager.Instance.Return(item);
            }

            _items.Clear();
        }

        private void UpdateCapacityText()
        {
            capacityText.text = $"{Save.unlocked.Count}/{Save.maxSwordsCount}";
        }

        private void UpdateCombineText()
        {
            craftingText.SetVariable("value",
                $"({Save.GetMergeableCount(_selectedItem.Save)}/{Save.requiredCountForMerge})");
        }
    }
}