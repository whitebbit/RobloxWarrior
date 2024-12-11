using System.Collections;
using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Abilities.Scriptables;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Abilities.PlayerAbilities
{
    [CreateAssetMenu(fileName = "MeteorAbility", menuName = "Configs/Player/Player Abilities/Meteor",
        order = 0)]
    public class MeteorAbility : PlayerAbility
    {
        [Tab("Meteor")] 
        [SerializeField] private ParticleSystem meteorParticle;
        [SerializeField] private AbilityProjectile projectilePrefab;
        [SerializeField] private float distanceBetweenPlayer;


        protected override void PerformAbility(IAbilityContext context)
        {
            context.Unit.StartCoroutine(SpawnMeteor(context));
        }

        private IEnumerator SpawnMeteor(IAbilityContext context)
        {
            var particle = Instantiate(meteorParticle);
            var position = context.Unit.transform.position + context.Unit.transform.forward * distanceBetweenPlayer;
            particle.transform.position = position;

            yield return new WaitForSeconds(meteorParticle.main.duration * 0.25f);

            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = position;
            projectile.Initialize(Vector3.zero, Damage, 0, 0.25f);
            Destroy(particle.gameObject, meteorParticle.main.duration * 0.75f);
        }
    }
}