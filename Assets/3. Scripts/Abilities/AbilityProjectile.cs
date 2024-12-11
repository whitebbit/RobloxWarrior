using System;
using _3._Scripts.Bots;
using _3._Scripts.Extensions;
using _3._Scripts.Pool;
using _3._Scripts.Units;
using _3._Scripts.Units.HitBoxes;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
    }
}