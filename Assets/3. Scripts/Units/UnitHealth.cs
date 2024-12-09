using System;
using _3._Scripts.Units.Interfaces;
using UnityEngine;

namespace _3._Scripts.Units
{
    public class UnitHealth
    {
        private float _maxHealth;

        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                var currentHealthPercentage = _currentHealth / _maxHealth;

                _maxHealth = Mathf.Clamp(value, 1, float.MaxValue);

                Health = _maxHealth * currentHealthPercentage;

                if (Health > MaxHealth)
                {
                    Health = MaxHealth;
                }
            }
        }

        private float _currentHealth;
        public event Action<float, float> OnHealthChanged;

        public float Health
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);

                OnHealthChanged?.Invoke(_currentHealth, MaxHealth);

                if (_currentHealth <= 0)
                {
                    _dying?.Die();
                }
            }
        }

        private readonly IDying _dying;

        public UnitHealth(float baseHealth, IDying dying)
        {
            _maxHealth = baseHealth;
            _currentHealth = baseHealth;
            _dying = dying;
        }

        public void UpdateValues(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;

            MaxHealth = _maxHealth;
        }
    }
}