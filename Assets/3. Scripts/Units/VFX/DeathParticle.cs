using System;
using System.Collections;
using _3._Scripts.Pool;
using _3._Scripts.Pool.Interfaces;
using UnityEngine;

namespace _3._Scripts.Units.VFX
{
    public class DeathParticle : MonoBehaviour, IPoolable
    {
        private ParticleSystem _particle;

        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        public void OnSpawn()
        {
            _particle?.Play();
            StartCoroutine(DelayDespawn());
        }

        public void OnDespawn()
        {
            _particle?.Stop();
        }

        private IEnumerator DelayDespawn()
        {
            yield return new WaitForSeconds(_particle.main.duration + 0.5f);
            ObjectsPoolManager.Instance.Return(this);
        }
    }
}