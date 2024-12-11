using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Units
{
    public class UnitHealthBar : MonoBehaviour
    {
        [SerializeField] private Unit unit; // Ссылка на Unit должна быть настроена в инспекторе
        [SerializeField] private Slider slider; // Ползунок для отображения здоровья
        [SerializeField] private TMP_Text healthText; // Текстовое поле для отображения здоровья

        private void Start()
        {
            unit.Health.OnHealthChanged += OnHealthChanged; // Подписка на событие
            OnHealthChanged(unit.Health.Health, unit.Health.MaxHealth); // Обн
        }
        
        private void OnHealthChanged(float currentHealth, float maxHealth)
        {
            var value = currentHealth / maxHealth;
            slider.DOValue(value, 0.15f); 
            healthText.text = $"{(int)currentHealth}/{(int)maxHealth}";
        }
    }
}