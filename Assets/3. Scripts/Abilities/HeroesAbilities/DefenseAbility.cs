using System.Collections.Generic;
using _3._Scripts.Abilities.Enums;
using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Abilities.Scriptables;
using UnityEngine;

namespace _3._Scripts.Abilities.HeroesAbilities
{
    [CreateAssetMenu(fileName = "DefenseAbility", menuName = "Configs/Heroes/Abilities/Defense", order = 0)]
    public class DefenseAbility : HeroAbility, IOneTimeAbility
    {
        [SerializeField] private float duration;

        public override AbilityTrigger Trigger => AbilityTrigger.OnWaveStart;
        public override bool CanUse => base.CanUse && !Used;
        
        protected override Dictionary<string, object> DescriptionParameters()
        {
            return new Dictionary<string, object>
            {
                { "value", duration },
            };
        }

        
        public override void ResetAbility()
        {
            Used = false;
            Player.Player.Instance.Damageable.DisableInvulnerability();
        }

        protected override void PerformAbility()
        {
            Used = true;
            Player.Player.Instance.Damageable.SetInvulnerability(duration);
        }

        public bool Used { get; set; }
    }
}