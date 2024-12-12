using System;
using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Bots;
using _3._Scripts.Detectors;
using _3._Scripts.Detectors.OverlapSystem.Base;
using _3._Scripts.Extensions;
using _3._Scripts.Pool;
using _3._Scripts.Units;
using _3._Scripts.Units.HitBoxes;
using _3._Scripts.Units.Interfaces;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _3._Scripts.Abilities
{
    public class AbilityProjectile : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> particles = new();
        [SerializeField] private SphereOverlapDetector<IWeaponVisitor> detector;

        public List<ParticleSystem> Particles => particles;

        private float _speed;
        private Vector3 _direction;
        private float _damage;

        private bool _isMoving;
        private bool _periodicAttacks;
        private bool _canAttack;

        public void Initialize(Vector3 direction, float damage, float speed, float lifetime)
        {
            _direction = direction;
            _damage = damage;
            _speed = speed;
            _isMoving = true;

            Destroy(gameObject, lifetime);
        }

        public void Initialize(float damage, float attackInterval, float radius, float lifetime)
        {
            _damage = damage;
            _periodicAttacks = true;

            detector.OnFound += Attack;
            detector.SetRadius(radius);

            StartCoroutine(PerformPeriodicAttacks(lifetime, attackInterval));
        }

        private void Update()
        {
            if (!_isMoving) return;

            var moveDirection = _direction.normalized;
            transform.position += moveDirection * _speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_periodicAttacks) return;

            if (ItsBot(other, out var hitBox)) return;

            Attack(hitBox);
        }

        private void Attack(IWeaponVisitor hitBox)
        {
            if (hitBox == null) return;

            hitBox.Visit(_damage);

            var floatingText = ObjectsPoolManager.Instance.Get<FloatingText>();

            var textPosition = transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(2f, 3f), 0) -
                               transform.forward * Random.Range(0.5f, 1f);

            var gradient = new VertexGradient(
                new Color(0.9056604f, 0.1485433f, 0.1409754f),
                new Color(0.9056604f, 0.1485433f, 0.1409754f),
                new Color(0.972549f, 0.6722908f, 0.2431373f),
                new Color(0.972549f, 0.6722908f, 0.2431373f));

            floatingText.Initialize($"{(int)_damage}", textPosition);
            floatingText.SetGradient(gradient);
        }

        private static bool ItsBot(Collider other, out IWeaponVisitor hitBox)
        {
            if (other.gameObject.TryGetComponent(out Bot bot)) return !bot.TryGetComponent(out hitBox);
            hitBox = null;
            return true;
        }

        private IEnumerator PerformPeriodicAttacks(float duration, float interval)
        {
            var elapsed = 0f;
            while (elapsed < duration)
            {
                detector.FindTargets();
                yield return new WaitForSeconds(interval);
                elapsed += interval;
            }

            yield return new WaitForSeconds(0.15f);
            Destroy(gameObject);
        }
    }
}