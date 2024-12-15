using System.Collections.Generic;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Config;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Localization;
using _3._Scripts.Saves;
using _3._Scripts.UI.Elements.AbilityPanel;
using _3._Scripts.UI.Panels.Base;
using GBGamesPlugin;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public class AbilityPanel : CollectionPanel<AbilityItem, PlayerAbility, Image>
    {
        [SerializeField] private List<AbilityItem> items = new();
        [SerializeField] private LocalizeStringEvent skillPointsText;
        [Tab("Buttons")] [SerializeField] private Button unlockButton;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button unequipButton;
        [SerializeField] private Button evoluteButton;

        private AbilitiesSave Save => GBGames.saves.abilitiesSave;
        private string CurrentId => Save.unlocked.Count > 0 ? Save.unlocked[0].id : "";
        protected override PlayerAbility CurrentConfig => Configuration.Instance.GetAbility(CurrentId);

        public override void Initialize()
        {
            base.Initialize();
            Items.AddRange(items);

            skillPointsText.SetVariable("value",
                WalletManager.GetCurrency(CurrencyType.SkillPoints).Value.ConvertToWallet());
            WalletManager.GetCurrency(CurrencyType.SkillPoints).OnValueChanged += (_, newValue) =>
                skillPointsText.SetVariable("value", newValue.ConvertToWallet());

            PopulateList();
        }

        protected override void ConfigureButtons()
        {
            base.ConfigureButtons();

            unlockButton.onClick.AddListener(Unlock);
            upgradeButton.onClick.AddListener(Upgrade);
            evoluteButton.onClick.AddListener(Evolute);
            unequipButton.onClick.AddListener(Unequip);
        }

        protected override void OnOpen()
        {
            currentItem.Initialize(CurrentConfig);

            UpdateCapacityText();
        }

        protected override void PopulateList()
        {
            foreach (var item in Items)
            {
                item.OnSelect += OnItemSelected;
            }
        }

        protected override bool ItsCurrentItem(AbilityItem item)
        {
            return Save.IsSelected(item.Config.ID);
        }

        protected override void UpdateCapacityText()
        {
        }

        protected override void OnSelectItem(AbilityItem item)
        {
            base.OnSelectItem(item);
            currentItem.Initialize(item.Config);
            UpdateButtonsState(item);
        }

        private void UpdateButtonsState(AbilityItem item)
        {
            unlockButton.gameObject.SetActive(item.Config.CanUnlock());
            upgradeButton.gameObject.SetActive(item.Unlocked && item.Config.CanUpgrade());
            equipSelectedButton.gameObject.SetActive(item.Unlocked && !Save.IsSelected(item.Config.ID));
            unequipButton.gameObject.SetActive(item.Unlocked && Save.IsSelected(item.Config.ID));
            evoluteButton.gameObject.SetActive(item.Unlocked && item.Config.NeedToBreak());
        }

        private void Unlock()
        {
            SelectedItem.Config.Unlock();

            UpdateButtonsState(SelectedItem);
            
            currentItem.UpdateItem();
            SelectedItem.UpdateItem();
        }

        private void Upgrade()
        {
            SelectedItem.Config.Upgrade();

            UpdateButtonsState(SelectedItem);
            
            currentItem.UpdateItem();
            SelectedItem.UpdateItem();
        }

        private void Evolute()
        {
            SelectedItem.Config.Evolute();

            UpdateButtonsState(SelectedItem);
        }

        private void Unequip()
        {
            Save.Unselect(SelectedItem.Config.ID);

            UpdateButtonsState(SelectedItem);
        }

        protected override void EquipItem(AbilityItem item)
        {
            Save.Select(item.Config.ID);

            UpdateButtonsState(SelectedItem);
        }
    }
}