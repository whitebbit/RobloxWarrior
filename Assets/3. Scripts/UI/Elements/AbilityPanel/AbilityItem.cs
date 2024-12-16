using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Localization;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements.AbilityPanel
{
    public class AbilityItem : CollectionItem<PlayerAbility, AbilityItem, Image>
    {
        [Tab("Main")] [SerializeField] private bool viewEvolution;


        [SerializeField] private PlayerAbility currentConfig;
        [SerializeField] private LocalizeStringEvent title;
        [SerializeField] private LocalizeStringEvent description;

        [Tab("Unlock Parameters")] [SerializeField]
        private LocalizeStringEvent rebirthCountText;

        [SerializeField] private List<AbilityToUnlockItem> abilityToUnlockItems = new();
        [Tab("Components")] [SerializeField] private TMP_Text levelText;
        [SerializeField] private Transform lockedTransform;

        protected override AbilityItem Self => this;

        public bool Unlocked => GBGames.saves.abilitiesSave.Unlocked(Config.ID);


        public void DefaultInitialize()
        {
            if (currentConfig == null) return;
            Initialize(currentConfig);
        }

        public override void Initialize(PlayerAbility config)
        {
            if (config == null)
            {
                SetItemState(false);
                return;
            }

            SetItemState(true);
            Config = config;
            UpdateItem();
        }

        public void UpdateItem()
        {
            icon.sprite = Config.Icon;

            UpdateLockedState();
            UpdateDescription();
            UpdateTitle();
            UpdateLevel();
        }

        private void UpdateLevel()
        {
            if (!levelText) return;
            var text = Config.NeedToBreak() && !Config.MaxUpgraded && viewEvolution
                ? $"{Config.Save.level}/{Config.Save.maxLevel} > {Config.Save.level}/{Config.NextUpgrade.maxLevel}"
                : $"{Config.Save.level}/{Config.Save.maxLevel}";
            levelText.text = text;
        }

        private void UpdateUnlockParameters()
        {
            levelText?.gameObject.SetActive(Unlocked);
            UnlockItemsState(false);
            rebirthCountText?.gameObject.SetActive(!Config.CanUnlock());
            UnlockItemsState(false);
            rebirthCountText?.gameObject.SetActive(false);

            if (Unlocked || Config.CanUnlock()) return;
            
            rebirthCountText?.gameObject.SetActive(true);
            rebirthCountText?.SetVariable("value", Config.RebornCountToUnlock);
            UpdateUnlockItems();
        }

        private void UnlockItemsState(bool state)
        {
            foreach (var item in abilityToUnlockItems)
            {
                item.gameObject.SetActive(state);
            }
        }

        private void UpdateUnlockItems()
        {
            var unlockItemCount = abilityToUnlockItems.Count;
            var i = 0;
            foreach (var ability in Config.AbilitiesToUnlock.TakeWhile(_ => i < unlockItemCount))
            {
                abilityToUnlockItems[i].gameObject.SetActive(true);
                abilityToUnlockItems[i].Initialize(ability.Icon, ability.TitleID, Config.AbilityLevelToUnlock);
                i++;
            }
        }

        private void UpdateTitle()
        {
            if (!title) return;
            title.SetReference(Config.TitleID);
        }

        private void UpdateDescription()
        {
            if (!description) return;
            description.SetReference(Config.DescriptionID);

            description.SetVariable("value", Config.GetDescriptionParameters<float>("value"));
            description.SetVariable("cooldown", Config.GetDescriptionParameters<float>("cooldown"));
            description.SetVariable("interval", Config.GetDescriptionParameters<float>("interval"));
            description.SetVariable("duration", Config.GetDescriptionParameters<float>("duration"));
        }

        private void UpdateLockedState()
        {
            if (lockedTransform)
                lockedTransform.gameObject.SetActive(!Unlocked);

            UpdateUnlockParameters();
        }

        private void SetItemState(bool state)
        {
            icon.gameObject.SetActive(state);

            if (description)
                description.gameObject.SetActive(state);

            if (title)
                title.gameObject.SetActive(state);

            UnlockItemsState(state);

            rebirthCountText?.gameObject.SetActive(state);
            levelText.gameObject.SetActive(state);
        }
    }
}