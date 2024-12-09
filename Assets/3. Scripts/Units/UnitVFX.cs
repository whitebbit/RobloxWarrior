using System;
using _3._Scripts.Extensions;
using _3._Scripts.Pool;
using _3._Scripts.Units.VFX;
using UnityEngine;

namespace _3._Scripts.Units
{
    public class UnitVFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem hitParticle;
        [SerializeField] private ParticleSystem levelUpParticle;
        [SerializeField] private Transform deathParticlePoint;
        [SerializeField] private ParticleSystem defenseParticle;


        private void Update()
        {
            if (_invulnerabilityCheck != null && _invulnerabilityCheck.Invoke())
                DisableInvulnerability();
        }

        public void OnHit()
        {
            if (hitParticle)
                hitParticle.Play();
        }

        public void OnLevelUp()
        {
            if (levelUpParticle)
                levelUpParticle.Play();
        }

        public void OnDeath()
        {
            var item = ObjectsPoolManager.Instance.Get<DeathParticle>();
            item.transform.position = deathParticlePoint.position;
        }

        private bool _invulnerabilityState;
        private Func<bool> _invulnerabilityCheck;

        public void SetInvulnerability(Func<bool> invulnerabilityCheck)
        {
            if (_invulnerabilityState) return;

            _invulnerabilityCheck = invulnerabilityCheck;
            _invulnerabilityState = true;
            defenseParticle.SetState(_invulnerabilityState);
        }

        public void DisableInvulnerability()
        {
            if (!_invulnerabilityState) return;

            _invulnerabilityState = false;
            defenseParticle.SetState(_invulnerabilityState);
        }
    }
}