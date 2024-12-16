using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Saves;
using _3._Scripts.Sounds;
using _3._Scripts.UI.Enums;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class PlayerStats
    {
        private static PlayerStatsConfig Config => Configuration.Instance.Config.PlayerConfig.StatsConfig;
        private static PlayerStatsSave Save => GBGames.saves.stats;

        #region Experience

        public event Action<int> OnLevelChange;
        public event Action<float> OnExperienceChanged;

        public float Experience
        {
            get => Save.experience;
            set
            {
                if (value > Save.experience)
                {
                    AddBoostedExperience(value - Save.experience);
                }
                else
                {
                    Save.experience = value;
                    OnExperienceChanged?.Invoke(Save.experience);
                }
            }
        }

        private void AddBoostedExperience(float addedExperience)
        {
            var boosted = addedExperience + addedExperience * ExperienceIncrease() / 100f;
            Save.experience += boosted;
            OnExperienceChanged?.Invoke(Save.experience);

            while (Save.experience >= ExperienceToLevelUp())
            {
                Save.experience -= ExperienceToLevelUp();
                Level += 1;
                UpgradePoints += 1;
                AudioManager.Instance.PlaySound("level_up");
                OnLevelChange?.Invoke(Level);
                OnExperienceChanged?.Invoke(Save.experience);
            }
        }

        public int Level
        {
            get => Save.level;
            private set => Save.level = value;
        }

        public event Action<int> OnUpgradePointsChanged;

        public int UpgradePoints
        {
            get => Save.upgradePoints;
            private set
            {
                Save.upgradePoints = value;
                OnUpgradePointsChanged?.Invoke(Save.upgradePoints);
            }
        }

        public float ExperienceToLevelUp()
        {
            var additionalOffset = Level / Config.OffsetIncrementFrequency * Config.OffsetIncrementValue;
            return Mathf.RoundToInt(Config.BaseExperience *
                                    (Config.ExperienceMultiplier + (Config.LevelOffset + additionalOffset) * Level));
        }

        #endregion

        #region Improvement

        public event Action OnStatsChanged;

        public event Action OnHealthImproving;
        public event Action OnAttackImproving;
        public event Action OnSpeedImproving;
        public event Action OnCritImproving;

        private int _additionalHealthPoint;

        public int HealthPoints
        {
            private get => Save.healthPoints;
            set
            {
                if (Save.healthPoints == value) return;

                Save.healthPoints = value;
                OnHealthImproving?.Invoke();
            }
        }

        private int _additionalAttackPoints;

        public int AttackPoints
        {
            private get => Save.attackPoints;
            set
            {
                Save.attackPoints = value;
                OnAttackImproving?.Invoke();
            }
        }

        private int _additionalSpeedPoints;

        public int SpeedPoints
        {
            private get => Save.speedPoints;
            set
            {
                Save.speedPoints = value;
                OnSpeedImproving?.Invoke();
            }
        }

        private int _additionalCritPoints;

        public int CritPoints
        {
            private get => Save.critPoints;
            set
            {
                Save.critPoints = value;
                OnCritImproving?.Invoke();
            }
        }

        public float HealthImprovement => (HealthPoints + _additionalHealthPoint) * Config.HealthImprovement;
        public float AttackImprovement => (AttackPoints + _additionalAttackPoints) * Config.AttackImprovement;
        public float SpeedImprovement => (SpeedPoints + _additionalSpeedPoints) * Config.SpeedImprovement;
        public float CritImprovement => (CritPoints + _additionalCritPoints) * Config.CritImprovement;

        public void UpgradeStats(ModificationType type, int amount)
        {
            if (UpgradePoints <= 0) return;

            var trueAmount = amount > UpgradePoints ? UpgradePoints : amount;

            switch (type)
            {
                case ModificationType.Health:
                    HealthPoints += trueAmount;
                    break;
                case ModificationType.Attack:
                    AttackPoints += trueAmount;
                    break;
                case ModificationType.Speed:
                    SpeedPoints += trueAmount;
                    break;
                case ModificationType.Crit:
                    CritPoints += trueAmount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            UpgradePoints -= trueAmount;
            OnStatsChanged?.Invoke();
        }

        public void AddAdditionalPoints(ModificationType type, int amount)
        {
            switch (type)
            {
                case ModificationType.Health:
                    _additionalHealthPoint += amount;
                    OnHealthImproving?.Invoke();
                    break;
                case ModificationType.Attack:
                    _additionalAttackPoints += amount;
                    OnAttackImproving?.Invoke();
                    break;
                case ModificationType.Speed:
                    _additionalSpeedPoints += amount;
                    OnSpeedImproving?.Invoke();
                    break;
                case ModificationType.Crit:
                    _additionalCritPoints += amount;
                    OnCritImproving?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            OnStatsChanged?.Invoke();
        }

        public void RemoveAdditionalPoints(ModificationType type, int amount)
        {
            switch (type)
            {
                case ModificationType.Health:
                    _additionalHealthPoint -= amount;
                    OnHealthImproving?.Invoke();
                    break;
                case ModificationType.Attack:
                    _additionalAttackPoints -= amount;
                    OnAttackImproving?.Invoke();
                    break;
                case ModificationType.Speed:
                    _additionalSpeedPoints -= amount;
                    OnSpeedImproving?.Invoke();
                    break;
                case ModificationType.Crit:
                    _additionalCritPoints -= amount;
                    OnCritImproving?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            OnStatsChanged?.Invoke();
        }

        public int GetLevel(ModificationType type)
        {
            return type switch
            {
                ModificationType.Health => HealthPoints + _additionalHealthPoint,
                ModificationType.Attack => AttackPoints + _additionalAttackPoints,
                ModificationType.Speed => SpeedPoints + _additionalSpeedPoints,
                ModificationType.Crit => CritPoints + _additionalCritPoints,

                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public int GetPointsSpent()
        {
            return HealthPoints + AttackPoints + SpeedPoints + CritPoints;
        }

        public void ResetStats()
        {
            var temp = UpgradePoints + GetPointsSpent();
            Save.ResetStats();
            UpgradePoints = temp;
            OnStatsChanged?.Invoke();
        }

        #endregion

        #region Additional Statistics

        public float AttackSpeed { get; private set; } = 1;

        #endregion

        #region Rebirth

        public int RebirthCount
        {
            get => Save.rebirthCounts;
            private set => Save.rebirthCounts = value;
        }

        public float AttackPercentIncrease => Save.rebirthCounts * Config.AttackPercentIncrease;


        private readonly Dictionary<string, float> _additionalExperienceIncrease = new();
        private float AdditionalExperienceIncrease => _additionalExperienceIncrease.Values.Sum();

        public void AddAdditionalExperienceIncrease(string placement, float increase)
        {
            _additionalExperienceIncrease.TryAdd(placement, increase);
        }

        public void RemoveAdditionalExperienceIncrease(string placement)
        {
            _additionalExperienceIncrease.Remove(placement);
        }

        public float ExperienceIncrease(int rebirthCounts = -1)
        {
            if (rebirthCounts == -1)
                rebirthCounts = Save.rebirthCounts;

            if (rebirthCounts == 0)
            {
                return 0;
            }

            return Config.BaseExperiencePercentIncrease * Mathf.Pow(2, rebirthCounts - 1) +
                   AdditionalExperienceIncrease;
        }
        

        public void Rebirth()
        {
            RebirthCount += 1;
            WalletManager.GetCurrency(CurrencyType.SkillPoints).Value += 1;

            Level = 1;
            Experience = 0;

            Save.ResetStats();
            UpdateLevelToNextRebirth();

            OnLevelChange?.Invoke(Level);
            OnStatsChanged?.Invoke();
        }

        public int LevelForRebirth
        {
            get => Save.levelForRebirth <= 0 ? Config.RebirthStartLevelRequired : Save.levelForRebirth;
            private set => Save.levelForRebirth = value;
        }

        private void UpdateLevelToNextRebirth()
        {
            var increaseCycles = RebirthCount / Config.RebirthIncreaseInterval;
            var l = increaseCycles > 0 ? increaseCycles * Config.RebirthIncreaseAmount : 0;
            LevelForRebirth += Config.RebirthIncreaseAmount + l;
        }

        #endregion
    }
}