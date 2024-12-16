using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Player;
using _3._Scripts.Sounds;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Abilities.PlayerAbilities
{
    [CreateAssetMenu(fileName = "DashAbility", menuName = "Configs/Player/Player Abilities/Dash",
        order = 0)]
    public class DashAbility : PlayerAbility
    {
        [Tab("Dash")] [SerializeField] private AbilityProjectile projectile;

        [SerializeField] private AnimationClip animation;

        [SerializeField] private float distance;
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;

        protected override void PerformAbility(IAbilityContext context)
        {
            if (!context.Unit.TryGetComponent(out PlayerMovement movement)) return;
            var p = Instantiate(projectile, context.Unit.transform, true);
            
            Completed = false;
            context.Animator.DoAnimation(animation, () =>
            {
                Player.Player.Instance.Controller.Enabled = true;
                Completed = true;
            });
            Player.Player.Instance.Controller.Enabled = false;

            p.transform.localPosition = Vector3.zero + Vector3.up;
            p.transform.localEulerAngles = Vector3.zero;
            p.Initialize(Vector3.zero, Damage, 0, lifeTime);
            movement.MoveInDirection(context.Unit.transform.forward, distance, speed);
            AudioManager.Instance.PlaySound(ID);

        }
    }
}