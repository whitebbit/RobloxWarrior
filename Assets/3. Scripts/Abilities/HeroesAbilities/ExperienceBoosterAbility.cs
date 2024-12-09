using System.Collections.Generic;
using _3._Scripts.Abilities.Enums;
using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Player;
using _3._Scripts.Units;
using UnityEngine;

namespace _3._Scripts.Abilities.HeroesAbilities
{
    [CreateAssetMenu(fileName = "ExperienceBoosterAbility", menuName = "Configs/Heroes/Abilities/Experience Booster", order = 0)]
    public class ExperienceBoosterAbility : HeroAbility, IOneTimeAbility
    {
        [SerializeField] private float boosterPercentage;

        public override AbilityTrigger Trigger => AbilityTrigger.OnWaveStart;
        public override bool CanUse => base.CanUse && !Used;
        private PlayerStats PlayerStats => Player.Player.Instance.Stats;
        private string BoosterPlacement => $"hero_ability_{boosterPercentage}_{GetHashCode()}";
        protected override Dictionary<string, object> DescriptionParameters()
        {
            return new Dictionary<string, object>
            {
                { "value", boosterPercentage },
            };
        }

        public override void ResetAbility()
        {
            Used = false;
            PlayerStats.RemoveAdditionalExperienceIncrease(BoosterPlacement);
        }

        protected override void PerformAbility()
        {
            Used = true;
            PlayerStats.AddAdditionalExperienceIncrease(BoosterPlacement, boosterPercentage);
        }

        public bool Used { get; set; }
    }
}