using System.Collections.Generic;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Panels.Base
{
    public abstract class CollectionPanel<TItem, TConfig> : UIPanel where TItem : CollectionItem<TConfig, TItem>
    {
        [SerializeField] private ScaleTransition transition;

        [Tab("Main Settings")] [SerializeField]
        protected TItem currentItem;

        [SerializeField] protected Transform container;
        [SerializeField] protected Button equipSelectedButton;
        [SerializeField] protected TMP_Text capacityText;

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }


        protected TItem SelectedItem;
        protected abstract TConfig CurrentConfig { get; }
        protected readonly List<TItem> Items = new();

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
            ConfigureButtons();
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            currentItem.Initialize(CurrentConfig);
            PopulateList();
            UpdateCapacityText();
        }

        /// <summary>
        /// Настройка обработчиков событий для кнопок.
        /// </summary>
        protected virtual void ConfigureButtons()
        {
            equipSelectedButton.onClick.AddListener(() => EquipItem(SelectedItem));
        }

        /// <summary>
        /// Заполнение списка.
        /// </summary>
        protected abstract void PopulateList();

        /// <summary>
        /// Обработчик выбора.
        /// </summary>
        protected virtual void OnItemSelected(TItem item)
        {
            SelectItem(item);
        }

        /// <summary>
        /// Установка текущего выбранного.
        /// </summary>
        protected void SelectItem(TItem item)
        {
            currentItem.Initialize(item.Config);

            if (SelectedItem != null)
            {
                if (!ItsCurrentItem(SelectedItem))
                    SelectedItem.DisableFocus();
                else
                    SelectedItem.SetCurrentFocus();
            }

            SelectedItem = item;
            SelectedItem.SetSelectedFocus();

            OnSelectItem(item);
        }

        protected abstract bool ItsCurrentItem(TItem item);

        protected virtual void OnSelectItem(TItem item)
        {
        }

        /// <summary>
        /// Обновление текста вместимости.
        /// </summary>
        protected abstract void UpdateCapacityText();

        /// <summary>
        /// Экипировка указанного.
        /// </summary>
        protected abstract void EquipItem(TItem item);
    }
}