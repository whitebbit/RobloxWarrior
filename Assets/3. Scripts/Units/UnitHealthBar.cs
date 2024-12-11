using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Units
{
    public class UnitHealthBar : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text healthText;

        private void Start()
        {
            unit.Health.OnHealthChanged += OnHealthChanged;
            OnHealthChanged(unit.Health.Health, unit.Health.MaxHealth);
        }

        private void OnHealthChanged(float arg1, float arg2)
        {
            var value = arg1 / arg2;
            slider.DOValue(value, 0.15f);
            if (healthText)
            {
                healthText.text = $"{(int)arg1}/{(int)arg2}";
            }
        }
    }
}