using System;
using UnityEngine;

namespace _3._Scripts.Units
{
    public sealed class UnitDamageable
    {
        private readonly UnitHealth _health;
        private readonly UnitVFX _vfx;

        public UnitDamageable(UnitHealth health, UnitVFX vfx)
        {
            _health = health;
            _vfx = vfx;
        }

        public void ApplyDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative.");

            var totalDamage = ProcessDamage(damage);

            if (totalDamage < 0)
                throw new ArgumentOutOfRangeException(nameof(totalDamage), "Damage cannot be negative.");

            _health.Health -= totalDamage;
            
            _vfx?.OnHit();
        }

        private float ProcessDamage(float damage)
        {
            return damage;
        }
    }
}