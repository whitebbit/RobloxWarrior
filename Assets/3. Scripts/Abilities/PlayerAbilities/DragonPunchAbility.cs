using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Abilities.Scriptables;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Abilities.PlayerAbilities
{
    [CreateAssetMenu(fileName = "DragonPunchAbility", menuName = "Configs/Player/Player Abilities/Dragon Punch",
        order = 0)]
    public class DragonPunchAbility : PlayerAbility
    {
        [Tab("Dragon Punch")] [SerializeField] private AnimationClip animation;
        [SerializeField] private AbilityProjectile projectilePrefab;
        [SerializeField] private float speed;
        [SerializeField] private float lifetime;

        protected override void PerformAbility(IAbilityContext context)
        {
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = context.Unit.transform.position + Vector3.up;
            projectile.transform.forward = context.Unit.transform.forward;
            projectile.Initialize(context.Unit.transform.forward, Damage, speed, lifetime);

            Completed = false;
            context.Animator.DoAnimation(animation, () =>
            {
                Completed = true;
                Player.Player.Instance.Controller.Enabled = true;
            });

            Player.Player.Instance.Controller.Enabled = false;
        }
    }
}