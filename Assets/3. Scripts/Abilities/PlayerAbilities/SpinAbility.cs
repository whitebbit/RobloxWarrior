﻿using System.Collections.Generic;
using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Extensions;
using _3._Scripts.Sounds;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Abilities.PlayerAbilities
{
    [CreateAssetMenu(fileName = "SpinAbility", menuName = "Configs/Player/Player Abilities/Spin",
        order = 0)]
    public class SpinAbility : PlayerAbility
    {
        [Tab("Spin")] [SerializeField] private AbilityProjectile projectilePrefab;

        [SerializeField] private AnimationClip animation;

        [SerializeField] private float duration;
        [SerializeField] private float interval;
        [SerializeField] private float radius;

        protected override Dictionary<string, object> DescriptionParameters()
        {
            var b = base.DescriptionParameters();
            b.Add("interval", interval);
            b.Add("duration", duration);
            return b;
        }

        protected override void PerformAbility(IAbilityContext context)
        {
            var projectile = Instantiate(projectilePrefab, context.Unit.transform, true);
            var particle = projectile.Particles;

            projectile.transform.localPosition = Vector3.zero + Vector3.up;
            if (particle.Count <= 0) return;

            var speed = animation.length / duration;
            AudioManager.Instance.PlaySound(ID);

            Completed = false;

            particle.SetDuration(duration + 0.5f);
            particle.SetState(true);

            projectile.Initialize(Damage, interval, radius, duration);
            context.Unit.Damageable.SetInvulnerability(duration);
            context.Animator.DoAnimation(animation, speed: speed, onComplete: () => { Completed = true; });
        }
    }
}