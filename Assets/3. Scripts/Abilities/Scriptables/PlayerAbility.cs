using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Abilities.Structs;
using _3._Scripts.Config.Scriptables;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Saves;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Abilities.Scriptables
{
    public abstract class PlayerAbility : ConfigObject<Sprite>, IAbility, ICooldownableAbility
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

        [Space] [SerializeField] private List<PlayerAbility> abilitiesToUnlock;
        [SerializeField] private int abilityLevelToUnlock;

        public int RebornCountToUnlock => rebornCountToUnlock;
        private Player.Player Player => _Scripts.Player.Player.Instance;
        public AbilitySave Save => GBGames.saves.abilitiesSave.Get(ID);

        public float Cooldown => cooldown;

        public List<PlayerAbility> AbilitiesToUnlock => abilitiesToUnlock;

        public int AbilityLevelToUnlock => abilityLevelToUnlock;

        protected virtual Dictionary<string, object> DescriptionParameters()
        {
            return new Dictionary<string, object>
            {
                { "cooldown", cooldown },
                { "value", baseDamagePercent },
            };
        }

        public T GetDescriptionParameters<T>(string valueName)
        {
            return (T)DescriptionParameters().GetValueOrDefault(valueName, default(T));
        }

        private float DamagePercent => baseDamagePercent *
                                       (1 + damageBoosterRatio * Level + damageBoosterBonus * (int)Math.Pow(Level, 2));

        protected float Damage => Player.GetTrueDamage(Player.Ammunition.Sword.GetTrueDamage()) * DamagePercent / 100f;

        private int Level => Save.level;
        public override Sprite Icon => icon;
        public bool CanUse => Time.time >= _lastUsedTime + cooldown;
        private float _lastUsedTime;

        public void Unlock()
        {
            if (!WalletManager.GetCurrency(CurrencyType.SkillPoints).TrySpend(1)) return;

            GBGames.saves.abilitiesSave.Unlock(this);
        }

        public void Upgrade()
        {
            if (!WalletManager.GetCurrency(CurrencyType.SkillPoints).TrySpend(1)) return;

            GBGames.saves.abilitiesSave.Upgrade(this);
        }

        public void Evolute()
        {
            var currentUpgrade = abilityUpgrades.FirstOrDefault(a => a.maxLevel == Save.maxLevel);
            var nextUpgrade = abilityUpgrades[abilityUpgrades.IndexOf(currentUpgrade) + 1];

            if (!WalletManager.GetCurrency(CurrencyType.Crystal).TrySpend(nextUpgrade.priceToBreak)) return;

            Save.maxLevel = nextUpgrade.maxLevel;
        }

        public bool CanUpgrade()
        {
            if (!GBGames.saves.abilitiesSave.Unlocked(ID)) return false;

            var currentUpgrade = abilityUpgrades.FirstOrDefault(a => a.maxLevel == Save.maxLevel);

            if (abilityUpgrades.IndexOf(currentUpgrade) >= abilityUpgrades.Count - 1) return false;

            return Level != currentUpgrade.maxLevel;
        }

        public AbilityUpgrade CurrentUpgrade => abilityUpgrades.FirstOrDefault(a => a.maxLevel == Save.maxLevel);

        public bool MaxUpgraded => abilityUpgrades.IndexOf(CurrentUpgrade) >= abilityUpgrades.Count - 1 &&
                                   Level == CurrentUpgrade.maxLevel;

        public bool NeedToBreak()
        {
            if (MaxUpgraded) return false;

            return Level == CurrentUpgrade.maxLevel;
        }


        public bool CanUnlock()
        {
            if (GBGames.saves.abilitiesSave.Unlocked(ID)) return false;

            if (abilitiesToUnlock.Count > 0)
            {
                return abilitiesToUnlock.All(a => a.Level >= abilityLevelToUnlock) &&
                       GBGames.saves.stats.rebirthCounts >= rebornCountToUnlock;
            }

            return GBGames.saves.stats.rebirthCounts >= rebornCountToUnlock;
        }

        public event Action<float> OnUseAbility;

        public bool Completed { get; set; } = true;
        public void ResetOnUseAbility() => OnUseAbility = null;

        public void UseAbility(IAbilityContext context)
        {
            if (!CanUse) return;

            PerformAbility(context);
            _lastUsedTime = Time.time;
            OnUseAbility?.Invoke(_lastUsedTime);
        }

        public void ResetAbility()
        {
            _lastUsedTime = Time.time;
        }

        protected abstract void PerformAbility(IAbilityContext context);
    }
}