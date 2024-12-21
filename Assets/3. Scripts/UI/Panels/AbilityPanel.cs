using System.Linq;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Config;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Game;
using _3._Scripts.Localization;
using _3._Scripts.Saves;
using _3._Scripts.UI.Elements.AbilityPanel;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VInspector;
using YG;

namespace _3._Scripts.UI.Panels
{
    public class AbilityPanel : CollectionPanel<AbilityItem, PlayerAbility, Image>
    {
        [SerializeField] private LocalizeStringEvent skillPointsText;
        [SerializeField] private LocalizeStringEvent evolutePriceText;
        [Tab("Buttons")] [SerializeField] private Button unlockButton;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button unequipButton;
        [SerializeField] private Button evoluteButton;

        private AbilitiesSave Save => YG2.saves.abilitiesSave;
        private string CurrentId => Save.selected.Count > 0 ? Save.selected[0].id : "";
        protected override PlayerAbility CurrentConfig => Configuration.Instance.GetAbility(CurrentId);

        public override void Initialize()
        {
            base.Initialize();


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
            UpdateButtonsState(CurrentConfig == null ? null : currentItem);
            UpdateCapacityText();

            foreach (var item in Items)
            {
                if (Save.IsSelected(item.Config.ID))
                {
                    item.SetCurrentFocus();
                }
                else
                    item.DisableFocus();
            }

            if (CurrentConfig != null)
                SelectedItem = Items.FirstOrDefault(a => a.Config.ID == CurrentConfig.ID);
        }

        protected override void PopulateList()
        {
            Items = container.GetComponentsInChildren<AbilityItem>().ToList();
            foreach (var item in Items)
            {
                item.DefaultInitialize();
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
            var itemNotNull = item != null;

            unlockButton.gameObject.SetActive(itemNotNull && item.Config.CanUnlock());
            upgradeButton.gameObject.SetActive(itemNotNull && item.Unlocked && !item.Config.NeedToBreak() &&
                                               !item.Config.MaxUpgraded);
            equipSelectedButton.gameObject.SetActive(itemNotNull && item.Unlocked && !Save.IsSelected(item.Config.ID));
            unequipButton.gameObject.SetActive(itemNotNull && item.Unlocked && Save.IsSelected(item.Config.ID));
            evoluteButton.gameObject.SetActive(itemNotNull && item.Unlocked && item.Config.NeedToBreak());

            if (itemNotNull && item.Config.CurrentUpgrade != null)
                evolutePriceText.SetVariable("value", (int)item.Config.CurrentUpgrade.priceToBreak);
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

            currentItem.UpdateItem();
            SelectedItem.UpdateItem();
        }

        private void Unequip()
        {
            if (!Save.IsSelected(SelectedItem.Config.ID)) return;
            
            Save.Unselect(SelectedItem.Config.ID);
            SelectedItem.DisableFocus();
            UpdateButtonsState(SelectedItem);
        }

        protected override void EquipItem(AbilityItem item)
        {
            if (!Save.CanSelect) return;

            Save.Select(item.Config.ID);
            SelectedItem.SetCurrentFocus();
            UpdateButtonsState(SelectedItem);
            
            YG2.SaveProgress();
        }
    }
}