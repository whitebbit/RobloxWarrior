using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Abilities.Structs;
using _3._Scripts.Config.Scriptables;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Saves;
using _3._Scripts.Saves.Interfaces;
using _3._Scripts.UI.Interfaces;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Abilities.Scriptables
{
    public abstract class Ability : ConfigObject<Sprite>
    {
        [SerializeField] private Sprite icon;

        [Tab("Ability")] [Header("Base settings")] [SerializeField]
        private float cooldown;

        [Space] [SerializeField] private float baseDamagePercent;
        [SerializeField] private float damageBoosterRatio = 0.1f;
        [SerializeField] private float damageBoosterBonus = 0.01f;
        [Space] [SerializeField] private List<AbilityUpgrade> abilityUpgrades = new();

        [Header("Unlock settings")] [SerializeField]
        private int rebornCountToUnlock;

        [Space] [SerializeField] private int abilityLevelToUnlock;
        [SerializeField] private List<Ability> abilitiesToUnlock;

        private AbilitySave Save => GBGames.saves.abilitiesSave.Get(ID);

        public float Cooldown => cooldown;

        public float DamagePercent => baseDamagePercent *
                                      (1 + damageBoosterRatio * Level + damageBoosterBonus * (int)Math.Pow(Level, 2));

        public int Level => Save.level;

        public override Sprite Icon => icon;
        private bool CanUse => Time.time >= _lastUsedTime + cooldown;
        private float _lastUsedTime;

        public void Unlock()
        {
            GBGames.saves.abilitiesSave.Unlock(this);
        }
        
        public void Upgrade()
        {
            Save.level += 1;
            Player.Player.Instance.Stats.SkillPoints -= 1;
        }

        public bool CanUpgrade()
        {
            var currentUpgrade = abilityUpgrades.FirstOrDefault(a => a.maxLevel == Save.maxLevel);

            if (abilityUpgrades.IndexOf(currentUpgrade) >= abilityUpgrades.Count - 1) return false;

            return Level != currentUpgrade.maxLevel;
        }

        public bool NeedToBreak()
        {
            var currentUpgrade = abilityUpgrades.FirstOrDefault(a => a.maxLevel == Save.maxLevel);

            if (abilityUpgrades.IndexOf(currentUpgrade) >= abilityUpgrades.Count - 1) return false;

            return Level == currentUpgrade.maxLevel;
        }

        public void Break()
        {
            var currentUpgrade = abilityUpgrades.FirstOrDefault(a => a.maxLevel == Save.maxLevel);
            var nextUpgrade = abilityUpgrades[abilityUpgrades.IndexOf(currentUpgrade) + 1];

            if (!WalletManager.TrySpend(CurrencyType.Crystal, nextUpgrade.priceToBreak)) return;

            Save.maxLevel = nextUpgrade.maxLevel;
        }

        public bool CanUnlock()
        {
            if (abilitiesToUnlock.Count > 0)
            {
                return abilitiesToUnlock.All(a => a.Level >= abilityLevelToUnlock) &&
                       GBGames.saves.stats.rebirthCounts >= rebornCountToUnlock;
            }

            return GBGames.saves.stats.rebirthCounts >= rebornCountToUnlock;
        }


        public void UseAbility()
        {
            if (!CanUse) return;

            PerformAbility();
            _lastUsedTime = Time.time;
        }

        protected abstract void PerformAbility();
    }
}