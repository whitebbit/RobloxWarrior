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
    }
}