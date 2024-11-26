using System;
using UnityEngine;

namespace _3._Scripts.Units
{
    public class UnitDamageable
    {
        private readonly UnitHealth _health;

        public UnitDamageable(UnitHealth health)
        {
            _health = health;
        }

        public void ApplyDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative.");

            var totalDamage = ProcessDamage(damage);
            
            if (totalDamage < 0)
                throw new ArgumentOutOfRangeException(nameof(totalDamage), "Damage cannot be negative.");

            _health.Health -= totalDamage;
        }

        protected virtual float ProcessDamage(float damage)
        {
            return damage;
        }
    }
}