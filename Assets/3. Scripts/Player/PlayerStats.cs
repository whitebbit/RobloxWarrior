﻿using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Saves;
using _3._Scripts.Sounds;
using _3._Scripts.UI.Enums;
using UnityEngine;
using YG;

namespace _3._Scripts.Player
{
    public class PlayerStats
    {
        private static PlayerStatsConfig Config => Configuration.Instance.Config.PlayerConfig.StatsConfig;
        private static PlayerStatsSave Save => YG2.saves.stats;

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

        private readonly int[] _levels =
        {
            1, 2, 3, 4, 5, 6, 7, 9, 10, 13, 15, 16, 17, 18, 20, 23, 27, 29, 30, 31, 32, 36, 37, 38, 40, 41, 42, 45, 46,
            50, 52, 55, 56, 61, 63, 78, 80, 85, 86, 89, 90, 91, 93, 98, 99, 100, 101, 102
        };

        private readonly float[] _experience =
        {
            25, 29, 32, 36, 41, 46, 51, 63, 70, 96, 118, 130, 144, 159, 194, 261, 386, 468, 516, 568, 626, 920, 1010,
            1110, 1350, 1480, 1630, 2170, 2390, 3510, 4250, 5660, 6230, 10040, 12150, 50770, 61440, 98960, 108850,
            144890, 159380, 175320, 212140, 341660, 375820, 413410, 454750, 500220
        };

        public float ExperienceToLevelUp()
        {
            var level = Level;
            if (level <= _levels[0])
                return ExtrapolateExperience(_levels[0], _experience[0], _levels[1], _experience[1], level);
            if (level >= _levels[^1])
                return ExtrapolateExperience(_levels[^2], _experience[^2], _levels[^1], _experience[^1], level);

            for (var i = 0; i < _levels.Length - 1; i++)
            {
                if (level >= _levels[i] && level <= _levels[i + 1])
                {
                    return CubicInterpolation(_levels[i], _experience[i], _levels[i + 1], _experience[i + 1], level);
                }
            }

            return 0;
        }

        private float CubicInterpolation(int x0, float y0, int x1, float y1, int x)
        {
            var t = (float)(x - x0) / (x1 - x0);

            return y0 + t * (y1 - y0);
        }

        private float ExtrapolateExperience(int x0, float y0, int x1, float y1, int x)
        {
            var t = (float)(x - x0) / (x1 - x0);
            return y0 + t * (y1 - y0);
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
            YG2.SetLeaderboard("rebirth", RebirthCount);
            WalletManager.GetCurrency(CurrencyType.SkillPoints).Value += 1;

            Level = 1;
            Experience = 0;
            UpgradePoints = 0;
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