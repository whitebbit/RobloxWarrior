using System;
using System.Collections;
using _3._Scripts.Abilities;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Config.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class AbilityWidget : MonoBehaviour, IInitializable<PlayerAbility>
    {
        [SerializeField] private Slider cooldownSlider;
        [SerializeField] private TMP_Text cooldownText;
        [SerializeField] private Image icon;
        [SerializeField] private Transform locked;

        private PlayerAbility _config;
        public bool Locked { get; set; }

        private void Awake()
        {
            var b = GetComponent<Button>();
            b?.onClick.AddListener(() =>
            {
                if (Locked)
                {
                    //TODO: покупка слота для способности
                }
            });
        }

        public void Initialize(PlayerAbility config)
        {
            StopAllCoroutines();

            SetLockedState(Locked);
            SetComponentsState(false);

            if (config == null)
            {
                icon.gameObject.SetActive(false);
                SetComponentsState(false);
                return;
            }

            icon.gameObject.SetActive(true);

            _config = config;
            icon.sprite = config.Icon;

            config.ResetOnUseAbility();
            config.OnUseAbility += OnUseAbility;

            OnUseAbility(Time.time);
        }

        private void SetLockedState(bool state)
        {
            locked.gameObject.SetActive(state);
        }

        private void SetComponentsState(bool state)
        {
            cooldownSlider.gameObject.SetActive(state);
            cooldownText.gameObject.SetActive(state);
        }

        private void OnUseAbility(float obj)
        {
            StartCoroutine(CooldownRoutine(obj));
        }

        private IEnumerator CooldownRoutine(float lastUsedTime)
        {
            cooldownSlider.gameObject.SetActive(true);
            cooldownText.gameObject.SetActive(true);
            cooldownSlider.maxValue = _config.Cooldown;
            cooldownSlider.value = _config.Cooldown;

            float elapsed = 0f;

            while (elapsed < _config.Cooldown)
            {
                elapsed = Time.time - lastUsedTime;

                float remainingTime = Mathf.Max(0, _config.Cooldown - elapsed);
                cooldownText.text = $"{remainingTime:F1}";
                cooldownSlider.value = remainingTime; // Reverse logic here

                yield return null;
            }

            cooldownSlider.gameObject.SetActive(false);
            cooldownText.gameObject.SetActive(false);
        }
    }
}