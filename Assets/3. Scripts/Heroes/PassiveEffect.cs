using System;
using _3._Scripts.Player;
using _3._Scripts.UI.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3._Scripts.Heroes
{
    [Serializable]
    public class PassiveEffect
    {
        [SerializeField] private ModificationType type;
        [SerializeField] private int points;

        public void ApplyEffect(PlayerStats stats)
        {
            switch (type)
            {
                case ModificationType.Health:
                    stats.AdditionalHealthPoint += points;
                    break;
                case ModificationType.Attack:
                    stats.AdditionalAttackPoints += points;
                    break;
                case ModificationType.Speed:
                    stats.AdditionalSpeedPoints += points;
                    break;
                case ModificationType.Crit:
                    stats.AdditionalCritPoints += points;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RemoveEffect(PlayerStats stats)
        {
            switch (type)
            {
                case ModificationType.Health:
                    stats.AdditionalHealthPoint -= points;
                    break;
                case ModificationType.Attack:
                    stats.AdditionalAttackPoints -= points;
                    break;
                case ModificationType.Speed:
                    stats.AdditionalSpeedPoints -= points;
                    break;
                case ModificationType.Crit:
                    stats.AdditionalCritPoints -= points;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}