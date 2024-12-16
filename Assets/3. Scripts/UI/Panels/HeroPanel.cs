using System;
using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Heroes;
using _3._Scripts.Heroes.Scriptables;
using _3._Scripts.Localization;
using _3._Scripts.Saves;
using _3._Scripts.Saves.Handlers;
using _3._Scripts.UI.Elements.HeroPanel;
using _3._Scripts.UI.Elements.SwordsPanel;
using _3._Scripts.UI.Panels.Base;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public class HeroPanel : CollectionPanel<HeroItem, HeroConfig, Image>
    {
        [SerializeField] private HeroItem prefab;
        [Tab("Buttons")] [SerializeField] private Button unlockButton;
        [SerializeField] private Button unequipButton;
        [SerializeField] private LocalizeStringEvent heroPointsText;

        protected override HeroConfig CurrentConfig =>
            Save.selected.Count <= 0 ? null : Configuration.Instance.GetHero(Save.selected[0].id);

        private List<HeroConfig> Configs => Configuration.Instance.Config.Heroes;
        private HeroesSave Save => GBGames.saves.heroesSave;

        public override void Initialize()
        {
            base.Initialize();
            PopulateList();

            heroPointsText.SetVariable("value",
                WalletManager.GetCurrency(CurrencyType.HeroPoints).Value.ConvertToWallet());
            WalletManager.GetCurrency(CurrencyType.HeroPoints).OnValueChanged += (_, newValue) =>
                heroPointsText.SetVariable("value", newValue.ConvertToWallet());
        }

        protected override void OnOpen()
        {
            currentItem.Initialize(CurrentConfig);
            if (CurrentConfig == null)
            {
                unlockButton.gameObject.SetActive(false);
                unequipButton.gameObject.SetActive(false);
            }
            else
            {
                unlockButton.gameObject.SetActive(false);
                unequipButton.gameObject.SetActive(true);
            }

            equipSelectedButton.gameObject.SetActive(false);

            UpdateCapacityText();
            UpdateList();
        }

        protected override void PopulateList()
        {
            foreach (var config in Configs)
            {
                var swordItem = Instantiate(prefab, container, true);
                swordItem.Initialize(config);
                swordItem.transform.localScale = Vector3.one;
                swordItem.OnSelect += OnItemSelected;

                if (Save.IsSelected(config.ID))
                {
                    swordItem.SetCurrentFocus();
                }

                Items.Add(swordItem);
            }
        }

        protected override void ConfigureButtons()
        {
            base.ConfigureButtons();
            unlockButton.onClick.AddListener(Unlock);
            unequipButton.onClick.AddListener(Unequip);
        }

        protected override void OnSelectItem(HeroItem item)
        {
            base.OnSelectItem(item);

            UpdateButtonsState(item);
        }

        private void UpdateButtonsState(HeroItem item)
        {
            unlockButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(false);
            equipSelectedButton.gameObject.SetActive(false);

            if (ItsCurrentItem(item))
            {
                unequipButton.gameObject.SetActive(true);
            }
            else if (item.Unlocked)
            {
                equipSelectedButton.gameObject.SetActive(true);
            }
            else
            {
                unlockButton.gameObject.SetActive(true);
            }
        }

        protected override bool ItsCurrentItem(HeroItem item)
        {
            return Save.IsSelected(item.Config.ID);
        }

        protected override void UpdateCapacityText()
        {
            capacityText.text = $"{Save.selected.Count}/{Save.capacity}";
        }

        private void Unlock()
        {
            if (!WalletManager.GetCurrency(CurrencyType.HeroPoints).TrySpend(1)) return;

            var save = new HeroSave
            {
                id = SelectedItem.Config.ID,
                level = 1,
            };

            Save.Unlock(save);
            UpdateList();
            UpdateButtonsState(SelectedItem);
        }

        private void UpdateList()
        {
            foreach (var item in Items)
            {
                item.UpdateItem();
            }
        }

        private void Unequip()
        {
            Save.Unselect(SelectedItem.Config.ID);

            UpdateButtonsState(SelectedItem);
            UpdateList();
            UpdateCapacityText();

            SelectedItem.DisableFocus();
            SelectedItem.SetSelectedFocus();
        }

        protected override void EquipItem(HeroItem item)
        {
            Save.Select(item.Config.ID);
            UpdateButtonsState(item);
            UpdateList();
            UpdateCapacityText();
        }
    }
}