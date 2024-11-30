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
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    /// <summary>
    /// Панель управления мечами, позволяющая экипировать, создавать, сортировать и удалять мечи.
    /// </summary>
    public class SwordsPanel : UIPanel
    {
        [Tab("Main Settings")] [SerializeField]
        private ScaleTransition transition;

        [SerializeField] private SwordItem currentSword;
        [SerializeField] private Transform container;
        [SerializeField] private TMP_Text capacityText;

        [Tab("Selected Sword Actions")] [SerializeField]
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

        /// <summary>
        /// Инициализация панели и настройка кнопок.
        /// </summary>
        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;

            ConfigureButtons();
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            currentSword.Initialize(Save.current);

            SetDeletingState(false);
            UpdateCapacityText();
            PopulateSwordList();
            SortSwords();
            UpdateCraftingText();
        }

        /// <summary>
        /// Настройка обработчиков событий для кнопок.
        /// </summary>
        private void ConfigureButtons()
        {
            deleteButton.onClick.AddListener(() => SetDeletingState(true));
            cancelDeleteButton.onClick.AddListener(() => SetDeletingState(false));
            acceptDeleteButton.onClick.AddListener(DeleteSelectedSwords);
            equipBestButton.onClick.AddListener(() =>
                EquipSword(_items.OrderByDescending(i => i.SwordDamage).FirstOrDefault()));
            equipSelectedButton.onClick.AddListener(() => EquipSword(_selectedItem));
            craftSelectedButton.onClick.AddListener(MergeSelectedSword);
            craftAllButton.onClick.AddListener(MergeAllSwords);
        }

        /// <summary>
        /// Заполнение списка мечей.
        /// </summary>
        private void PopulateSwordList()
        {
            ClearSwordList();

            foreach (var save in Save.unlocked)
            {
                var swordItem = ObjectsPoolManager.Instance.Get<SwordItem>();
                swordItem.Initialize(save);
                swordItem.transform.SetParent(container);
                swordItem.transform.localScale = Vector3.one;
                swordItem.OnSelect += OnSwordSelected;

                if (save.uid == Save.current.uid)
                {
                    _selectedItem = swordItem;
                    swordItem.SetCurrentFocus();
                }

                _items.Add(swordItem);
            }
        }

        /// <summary>
        /// Обработчик выбора меча.
        /// </summary>
        private void OnSwordSelected(SwordItem sword)
        {
            if (!_deleteMode)
            {
                SelectSword(sword);
            }
            else if (sword.Save.uid != Save.current.uid)
            {
                // Используем SetDeleteState для управления состоянием удаления
                sword.SetDeleteState(!sword.ItemToDelete);
            }
        }

        /// <summary>
        /// Установка текущего выбранного меча.
        /// </summary>
        private void SelectSword(SwordItem sword)
        {
            currentSword.Initialize(sword.Save);

            // Обновляем фокус текущего выбранного меча
            _selectedItem?.DisableFocus();
            _selectedItem = sword;
            _selectedItem.SetSelectedFocus();

            UpdateCraftingText();
        }

        /// <summary>
        /// Включение или отключение режима удаления мечей.
        /// </summary>
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

            var curr = _items.FirstOrDefault(i => i.Save.uid == Save.current.uid);
            if (curr != null)
                curr.SetDeleteState(true);
        }

        /// <summary>
        /// Удаление выбранных мечей.
        /// </summary>
        private void DeleteSelectedSwords()
        {
            var swordsToDelete = _items.Where(i => i.ItemToDelete).ToList();

            foreach (var sword in swordsToDelete)
            {
                Save.Delete(sword.Save);
                _items.Remove(sword);
            }

            SetDeletingState(false);
        }

        /// <summary>
        /// Сортировка списка мечей.
        /// </summary>
        private void SortSwords()
        {
            var sortedSwords = _items
                .OrderByDescending(s => s.SwordDamage)
                .ThenByDescending(s => s.Config.Rarity)
                .ToList();

            for (int i = 0; i < sortedSwords.Count; i++)
            {
                sortedSwords[i].transform.SetSiblingIndex(i);
            }
        }

        /// <summary>
        /// Экипировка указанного меча.
        /// </summary>
        private void EquipSword(SwordItem swordItem)
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

        /// <summary>
        /// Объединение текущего выбранного меча.
        /// </summary>
        private void MergeSelectedSword()
        {
            if (_selectedItem == null) return;

            Save.TryMergeObject(_selectedItem.Save);
            UpdateUIAfterMerge();
            EquipSword(_items.FirstOrDefault(i => i.Save.uid == Save.current.uid));
        }

        /// <summary>
        /// Объединение всех доступных мечей.
        /// </summary>
        private void MergeAllSwords()
        {
            Save.MergeAll();
            UpdateUIAfterMerge();
            EquipSword(_items.FirstOrDefault(i => i.Save.uid == Save.current.uid));
        }

        /// <summary>
        /// Обновление интерфейса после объединения.
        /// </summary>
        private void UpdateUIAfterMerge()
        {
            currentSword.UpdateItem();
            PopulateSwordList();
            SortSwords();
            UpdateCapacityText();
            UpdateCraftingText();
        }

        protected override void OnClose()
        {
            base.OnClose();
            ClearSwordList();
        }

        /// <summary>
        /// Очистка списка мечей.
        /// </summary>
        private void ClearSwordList()
        {
            foreach (var item in _items)
            {
                ObjectsPoolManager.Instance.Return(item);
            }

            _items.Clear();
        }

        /// <summary>
        /// Обновление текста вместимости.
        /// </summary>
        private void UpdateCapacityText()
        {
            capacityText.text = $"{Save.unlocked.Count}/{Save.maxSwordsCount}";
        }

        /// <summary>
        /// Обновление текста для объединения.
        /// </summary>
        private void UpdateCraftingText()
        {
            craftingText.SetVariable("value",
                $"({Save.GetMergeableCount(_selectedItem?.Save)}/{Save.requiredCountForMerge})");
        }
    }
}