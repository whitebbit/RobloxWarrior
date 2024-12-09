using System.Collections.Generic;
using _3._Scripts.Abilities.Enums;
using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Game;
using UnityEngine;

namespace _3._Scripts.Abilities.HeroesAbilities
{
    [CreateAssetMenu(fileName = "WaveSkipAbility", menuName = "Configs/Heroes/Abilities/Wave Skip", order = 0)]
    public class WaveSkipAbility : HeroAbility, IOneTimeAbility
    {
        [SerializeField] private int waveNumber;

        public override AbilityTrigger Trigger => AbilityTrigger.BeforeBattle;
        public override bool CanUse => base.CanUse && !Used;
        protected override Dictionary<string, object> DescriptionParameters()
        {
            return new Dictionary<string, object>
            {
                { "value", waveNumber },
            };
        }

        protected override void PerformAbility()
        {
            Used = true;
            GameContext.StartWaveNumber = waveNumber;
        }

        public override void ResetAbility()
        {
            Used = false;
            GameContext.StartWaveNumber = 0;
        }

        public bool Used { get; set; }
    }
}