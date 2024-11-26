using System;
using _3._Scripts.Config;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Saves;
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
                Save.experience = value;
                OnExperienceChanged?.Invoke(Save.experience);
                while (Save.experience >= ExperienceToLevelUp())
                {
                    Save.experience -= ExperienceToLevelUp();
                    Level += 1;
                    UpgradePoints += 1;

                    OnLevelChange?.Invoke(Level);
                    OnExperienceChanged?.Invoke(Save.experience);
                }
            }
        }

        public int Level
        {
            get => Save.level;
            set => Save.level = value;
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
            return Config.InitialExperienceCoefficient * Mathf.Pow(Level, Config.DifficultyFactor);
        }

        #endregion

        #region Improvement

        public event Action OnHealthImproving;
        public event Action OnAttackImproving;
        public event Action OnSpeedImproving;
        public event Action OnCritImproving;

        public int HealthPoints
        {
            private get => Save.healthPoints;
            set
            {
                Save.healthPoints = value;
                OnHealthImproving?.Invoke();
            }
        }

        public int AttackPoints
        {
            private get => Save.attackPoints;
            set
            {
                Save.attackPoints = value;
                OnAttackImproving?.Invoke();
            }
        }

        public int SpeedPoints
        {
            private get => Save.speedPoints;
            set
            {
                Save.speedPoints = value;
                OnSpeedImproving?.Invoke();
            }
        }

        public int CritPoints
        {
            private get => Save.critPoints;
            set
            {
                Save.critPoints = value;
                OnCritImproving?.Invoke();
            }
        }

        public float HealthImprovement => HealthPoints * Config.HealthImprovement;
        public float AttackImprovement => AttackPoints * Config.AttackImprovement;
        public float SpeedImprovement => SpeedPoints * Config.SpeedImprovement;
        public float CritImprovement => CritPoints * Config.CritImprovement;

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
        }

        public int GetLevel(ModificationType type)
        {
            return type switch
            {
                ModificationType.Health => HealthPoints,
                ModificationType.Attack => AttackPoints,
                ModificationType.Speed => SpeedPoints,
                ModificationType.Crit => CritPoints,

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

        public float ExperienceIncrease(int rebirthCounts = -1)
        {
            if (rebirthCounts == -1)
                rebirthCounts = Save.rebirthCounts;

            return Config.BaseExperiencePercentIncrease * Mathf.Pow(2, rebirthCounts - 1);
        }

        public int SkillPoints
        {
            get => Save.skillPoints;
            set => Save.skillPoints = value;
        }

        public void Rebirth()
        {
            RebirthCount += 1;
            SkillPoints += 1;

            Experience = 0;
            Level = 1;

            Save.ResetStats();
            UpdateLevelToNextRebirth();

            OnLevelChange?.Invoke(Level);
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