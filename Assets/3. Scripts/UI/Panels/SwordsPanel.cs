using System.Linq;
using _3._Scripts.Localization;
using _3._Scripts.Pool;
using _3._Scripts.Saves;
using _3._Scripts.Saves.Handlers;
using _3._Scripts.UI.Elements.SwordsPanel;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VInspector;
using YG;

namespace _3._Scripts.UI.Panels
{
    /// <summary>
    /// Панель управления мечами, позволяющая экипировать, создавать, сортировать и удалять мечи.
    /// </summary>
    public class SwordsPanel : CollectionPanel<SwordItem, SwordSave, RawImage>
    {
        [Tab("Selected Sword Actions")] [SerializeField]
        private Button craftSelectedButton;

        [SerializeField] private LocalizeStringEvent craftingText;

        [Tab("Control Buttons")] [SerializeField]
        private Transform deleteControlsButtonContainer;

        [SerializeField] private Button deleteButton;
        [SerializeField] private Button cancelDeleteButton;
        [SerializeField] private Button acceptDeleteButton;
        [Space] [SerializeField] private Button equipBestButton;
        [SerializeField] private Button craftAllButton;

        private SwordsSave Save => YG2.saves.swordsSave;
        private bool _deleteMode;

        protected override SwordSave CurrentConfig => Save.current;

        protected override void OnOpen()
        {
            currentItem.Initialize(CurrentConfig);
            SetDeletingState(false);
            UpdateCapacityText();
            PopulateList();
            SortSwords();
            UpdateCraftingText();
        }

        protected override void ConfigureButtons()
        {
            base.ConfigureButtons();
            deleteButton.onClick.AddListener(() => SetDeletingState(true));
            cancelDeleteButton.onClick.AddListener(() => SetDeletingState(false));
            acceptDeleteButton.onClick.AddListener(DeleteSelectedSwords);
            equipBestButton.onClick.AddListener(() =>
                EquipItem(Items.OrderByDescending(i => i.SwordDamage).FirstOrDefault()));
            craftSelectedButton.onClick.AddListener(MergeSelectedSword);
            craftAllButton.onClick.AddListener(MergeAllSwords);
        }

        protected override void PopulateList()
        {
            ClearSwordList();

            foreach (var save in Save.unlocked)
            {
                var swordItem = ObjectsPoolManager.Instance.Get<SwordItem>();
                swordItem.Initialize(save);
                swordItem.transform.SetParent(container);
                swordItem.transform.localScale = Vector3.one;
                swordItem.OnSelect += OnItemSelected;

                if (save.uid == Save.current.uid)
                {
                    SelectedItem = swordItem;
                    swordItem.SetCurrentFocus();
                }

                Items.Add(swordItem);
            }
        }

        protected override void OnItemSelected(SwordItem item)
        {
            if (!_deleteMode)
            {
                SelectItem(item);
            }
            else if (item.Config.uid != CurrentConfig.uid)
            {
                item.SetDeleteState(!item.ItemToDelete);
            }
        }

        protected override bool ItsCurrentItem(SwordItem item)
        {
            return CurrentConfig == item.Config;
        }

        protected override void OnSelectItem(SwordItem sword)
        {
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

            foreach (var item in Items.Where(i => i.Config.uid != CurrentConfig.uid))
            {
                item.DisableFocus();
                if (!state)
                    item.SetDeleteState(false);
            }

            var curr = Items.FirstOrDefault(i => i.Config.uid == CurrentConfig.uid);
            if (curr != null)
                curr.SetDeleteState(false);
        }

        /// <summary>
        /// Удаление выбранных мечей.
        /// </summary>
        private void DeleteSelectedSwords()
        {
            var swordsToDelete = Items.Where(i => i.ItemToDelete).ToList();

            foreach (var sword in swordsToDelete)
            {
                Save.Delete(sword.Config);
                Items.Remove(sword);
            }

            SetDeletingState(false);
        }

        /// <summary>
        /// Сортировка списка мечей.
        /// </summary>
        private void SortSwords()
        {
            var sortedSwords = Items
                .OrderByDescending(s => s.SwordDamage)
                .ThenByDescending(s => s.SwordConfig.Rarity)
                .ToList();

            for (int i = 0; i < sortedSwords.Count; i++)
            {
                sortedSwords[i].transform.SetSiblingIndex(i);
            }
        }

        /// <summary>
        /// Экипировка указанного меча.
        /// </summary>
        protected override void EquipItem(SwordItem item)
        {
            if (item == null) return;

            Save.SetCurrent(item.Config);
            currentItem.Initialize(item.Config);

            foreach (var i in Items)
            {
                i.DisableFocus();
            }

            item.SetCurrentFocus();
            YG2.SaveProgress();
        }

        /// <summary>
        /// Объединение текущего выбранного меча.
        /// </summary>
        private void MergeSelectedSword()
        {
            if (SelectedItem == null) return;

            Save.TryMergeObject(SelectedItem.Config);
            UpdateUIAfterMerge();
            EquipItem(Items.FirstOrDefault(i => i.Config.uid == CurrentConfig.uid));
        }

        /// <summary>
        /// Объединение всех доступных мечей.
        /// </summary>
        private void MergeAllSwords()
        {
            Save.MergeAll();
            UpdateUIAfterMerge();
            EquipItem(Items.FirstOrDefault(i => i.Config.uid == CurrentConfig.uid));
        }

        /// <summary>
        /// Обновление интерфейса после объединения.
        /// </summary>
        private void UpdateUIAfterMerge()
        {
            currentItem.UpdateItem();
            PopulateList();
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
            foreach (var item in Items)
            {
                ObjectsPoolManager.Instance.Return(item);
            }

            Items.Clear();
        }

        /// <summary>
        /// Обновление текста вместимости.
        /// </summary>
        protected override void UpdateCapacityText()
        {
            capacityText.text = $"{Save.unlocked.Count}/{Save.maxSwordsCount}";
        }

        /// <summary>
        /// Обновление текста для объединения.
        /// </summary>
        private void UpdateCraftingText()
        {
            craftingText.SetVariable("value",
                $"({Save.GetMergeableCount(SelectedItem?.Config)}/{Save.requiredCountForMerge})");
        }
    }
}