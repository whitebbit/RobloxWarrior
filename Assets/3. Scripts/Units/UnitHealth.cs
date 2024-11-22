using System;
using _3._Scripts.Units.Interfaces;
using UnityEngine;

namespace _3._Scripts.Units
{
    public class UnitHealth
    {
        public float MaxHealth { get; private set; }

        private float _currentHealth;
        private readonly float _baseHealth;

        private IDying _dying;

        public UnitHealth(float baseHealth, IDying dying)
        {
            _baseHealth = baseHealth;
            MaxHealth = _baseHealth;
            _currentHealth = _baseHealth;
            _dying = dying;
        }

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

        public void IncreaseMaxHealth(float amount)
        {
            var currentHealthPercentage = Health / MaxHealth;

            MaxHealth = _baseHealth + amount;
            Health = MaxHealth * currentHealthPercentage;

            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }

        public event Action<float, float> OnHealthChanged;
    }
}