using System.Collections.Generic;
using _3._Scripts.Abilities.Enums;
using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3._Scripts.Abilities.HeroesAbilities
{
    [CreateAssetMenu(fileName = "HealingAbility", menuName = "Configs/Heroes/Abilities/Healing", order = 0)]
    public class HealingAbility : HeroAbility, ICooldownableAbility
    {
        [SerializeField] private float healthPercentage;
        [SerializeField] private float cooldown;

        public float Cooldown => cooldown;
        public override AbilityTrigger Trigger => AbilityTrigger.DuringBattle;
        public override bool CanUse => base.CanUse && Time.time >= _lastUsedTime + cooldown;
        private UnitHealth PlayerHealth => Player.Player.Instance.Health;

        private float _lastUsedTime;

        protected override Dictionary<string, object> DescriptionParameters()
        {
            return new Dictionary<string, object>
            {
                { "cooldown", cooldown },
                { "value", healthPercentage },
            };
        }

        public override void ResetAbility()
        {
            _lastUsedTime = 0;
        }

        protected override void PerformAbility()
        {
            var healing = PlayerHealth.MaxHealth * healthPercentage / 100f;
            PlayerHealth.Health += healing;

            _lastUsedTime = Time.time;
        }
    }
}