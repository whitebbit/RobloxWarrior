using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Units
{
    public class UnitHealthBar : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField] private Slider slider;
        [Space] [SerializeField] private Transform headBone;
        [SerializeField] private Vector3 offset;

        private void LateUpdate()
        {
            if (headBone != null)
            {
                transform.position = headBone.position + offset;
            }
        }

        private void Start()
        {
            unit.Health.OnHealthChanged += OnHealthChanged;
            OnHealthChanged(unit.Health.Health, unit.Health.MaxHealth);
        }

        private void OnHealthChanged(float arg1, float arg2)
        {
            var value = arg1 / arg2;
            slider.DOValue(value, 0.15f);
        }
    }
}