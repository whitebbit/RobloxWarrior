using System;
using System.Collections.Generic;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Heroes.Scriptables;
using _3._Scripts.Pool.Interfaces;
using _3._Scripts.Units;
using UnityEngine;

namespace _3._Scripts.Heroes
{
    public class Hero : Unit, IInitializable<HeroConfig>, IPoolable
    {
        public override UnitHealth Health => _health ??= new UnitHealth(10000, null);

        private UnitHealth _health;
        private HeroMovement _movement;
        private HeroAnimator _animator;
        private HeroAbilityManager _abilityManager;

        private Transform _target;
        private readonly List<PassiveEffect> _passiveEffects = new();
        private HeroModel _model;

        protected override void OnAwake()
        {
            _movement = GetComponent<HeroMovement>();
            _abilityManager = GetComponent<HeroAbilityManager>();
            _animator = GetComponent<HeroAnimator>();
        }

        private void Update()
        {
            if (_target == null)
            {
                _movement.Move(Vector2.zero);
                return;
            }

            if (Vector3.Distance(_target.position, transform.position) >= 10f)
                _movement.Teleport(_target.position);

            var direction = _target.transform.position - transform.position;
            var trueDirection = new Vector2(direction.x, direction.z);
            _movement.Move(trueDirection);
        }

        public void SetTarget(Transform target) => _target = target;

        public void Initialize(HeroConfig config)
        {
            _model = Instantiate(config.ModelPrefab, transform);
            _model.transform.localPosition = Vector3.zero;

            _animator.Initialize(config);
            _movement.Initialize(config);
            _abilityManager.RegisterAbility(config.Ability);

            foreach (var effect in config.PassiveEffects)
            {
                effect.Reset();
                effect.ApplyEffect(Player.Player.Instance.Stats);
                _passiveEffects.Add(effect);
            }
        }

        public void OnSpawn()
        {
            _abilityManager.ClearAbilities();
            _target = null;
        }

        public void OnDespawn()
        {
            foreach (var effect in _passiveEffects)
            {
                effect.RemoveEffect(Player.Player.Instance.Stats);
            }

            _abilityManager.ClearAbilities();
            if (_model)
                Destroy(_model.gameObject);
        }
    }
}