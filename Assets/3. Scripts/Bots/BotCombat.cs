using System;
using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Animations.Structs;
using _3._Scripts.Bots.Sciptables;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Units;
using _3._Scripts.Weapons;
using _3._Scripts.Weapons.Base;
using _3._Scripts.Weapons.Configs;
using UnityEngine;

namespace _3._Scripts.Bots
{
    public class BotCombat : UnitCombat<Weapon<BaseWeaponConfig>, BaseWeaponConfig>, IInitializable<BotConfig>
    {
        [SerializeField] private Weapon<BaseWeaponConfig> weapon;

        private BotConfig _config;
        public override Weapon<BaseWeaponConfig> Weapon => weapon;
        protected override List<AttackAnimation> AttackAnimations => _config.AnimationConfig.AttackAnimations;
        protected override UnitAnimator Animator { get; set; }

        protected
            private void Awake()
        {
            Animator = GetComponent<BotAnimator>();
        }

        public void Initialize(BotConfig config)
        {
            _config = config;

            weapon.Initialize(new BaseWeaponConfig(_config.Damage));
        }

        public void ResetCooldown()
        {
            _isAttacking = false;
            _lastAttackTime = 0;
        }

        protected override bool CanAttack()
        {
            return base.CanAttack() && Time.time - _lastAttackTime >= _config.AttackSpeed;
        }

        private float _lastAttackTime;

        protected override IEnumerator PerformAttack()
        {
            _lastAttackTime = Time.time;
            return base.PerformAttack();
        }
    }
}