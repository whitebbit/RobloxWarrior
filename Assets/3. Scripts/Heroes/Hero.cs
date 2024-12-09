using System;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Heroes.Scriptables;
using _3._Scripts.Units;
using UnityEngine;

namespace _3._Scripts.Heroes
{
    public class Hero : Unit, IInitializable<HeroConfig>
    {
        [SerializeField] private HeroConfig config;

        public override UnitHealth Health => _health;
        private UnitHealth _health;
        private HeroMovement _movement;
        private HeroAnimator _animator;
        private HeroAbilityManager _abilityManager;


        protected override void OnAwake()
        {
            _health = new UnitHealth(10000, null);
            _movement = GetComponent<HeroMovement>();
            _abilityManager = GetComponent<HeroAbilityManager>();
            _animator = GetComponent<HeroAnimator>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            Initialize(config);
        }

        private void Update()
        {
            _movement.Move(Vector2.zero);
        }

        public void Initialize(HeroConfig config)
        {
            var model = Instantiate(config.ModelPrefab, transform);
            model.transform.localPosition = Vector3.zero;

            _animator.Initialize(config);
            _movement.Initialize(config);
            _abilityManager.RegisterAbility(config.Ability);

            foreach (var effect in config.PassiveEffects)
            {
                effect.ApplyEffect(Player.Player.Instance.Stats);
            }
        }
    }
}