using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Abilities.Scriptables;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Abilities.PlayerAbilities
{
    [CreateAssetMenu(fileName = "IceAbility", menuName = "Configs/Player/Player Abilities/Ice",
        order = 0)]
    public class IceAbility : PlayerAbility
    {
        [Tab("Ice")] [SerializeField] private AbilityProjectile projectilePrefab;

        protected override void PerformAbility(IAbilityContext context)
        {
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = context.Unit.transform.position;
            projectile.Initialize(Vector3.zero, Damage, 0, 2);
        }
    }
}