using UnityEngine;

namespace _3._Scripts.Units
{
    public class UnitVFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem hitParticle;
        [SerializeField] private ParticleSystem levelUpParticle;


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
    }
}