using System;
using _3._Scripts.Bots;
using _3._Scripts.Units;
using _3._Scripts.Units.HitBoxes;
using UnityEngine;

namespace _3._Scripts.Abilities
{
    public class AbilityProjectile : MonoBehaviour
    {
        private float _speed;

        private Vector3 _direction;
        private float _damage;

        public void Initialize(Vector3 direction, float damage, float speed, float lifetime)
        {
            _direction = direction;
            _damage = damage;
            _speed = speed;

            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            var moveDirection = _direction.normalized;

            transform.position += moveDirection * _speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out Bot bot)) return;

            if (!bot.TryGetComponent(out HitBox hitBox)) return;

            hitBox.Visit(_damage);
        }
    }
}