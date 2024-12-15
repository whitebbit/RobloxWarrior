using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Abilities;
using _3._Scripts.Animations;
using _3._Scripts.Animations.Structs;
using _3._Scripts.Config;
using _3._Scripts.Swords;
using _3._Scripts.Swords.Scriptables;
using _3._Scripts.Units;
using _3._Scripts.Weapons;
using _3._Scripts.Weapons.Base;
using _3._Scripts.Weapons.Interfaces;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class PlayerCombat : UnitCombat<Sword, SwordConfig>
    {
        public override Sword Weapon => _playerAmmunition.Sword;

        protected override List<AttackAnimation> AttackAnimations =>
            Configuration.Instance.Config.PlayerConfig.AnimationConfig.AttackAnimations;

        protected override UnitAnimator Animator { get; set; }
        protected override float AttackSpeed => Player.Instance.Stats.AttackSpeed;

        private PlayerMovement _playerMovement;
        private PlayerAmmunition _playerAmmunition;
        private AbilityContext _abilityContext;

        private void Awake()
        {
            Animator = GetComponent<PlayerAnimator>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAmmunition = GetComponent<PlayerAmmunition>();
            _abilityContext = new AbilityContext(Player.Instance, Animator);
        }

        protected override bool CanAttack()
        {
            return base.CanAttack() && _playerMovement.IsGrounded;
        }

        public void UseFirstAbility()
        {
            _playerAmmunition.GetPlayerAbility(0)?.UseAbility(_abilityContext);
        }

        public void UseSecondAbility()
        {
            _playerAmmunition.GetPlayerAbility(1)?.UseAbility(_abilityContext);
        }

        public void UseThirdAbility()
        {
            _playerAmmunition.GetPlayerAbility(2)?.UseAbility(_abilityContext);
        }
    }
}