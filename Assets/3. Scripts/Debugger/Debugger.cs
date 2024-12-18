using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using VInspector;
using YG;
using Random = UnityEngine.Random;

namespace _3._Scripts.Debugger
{
    public class Debugger : MonoBehaviour
    {
        [Tab("Enable Button")] [SerializeField]
        private Button enableButton;

        [SerializeField] private Transform enableArrow;

        [Tab("Quality")] [SerializeField] private Volume volume;

        [SerializeField] private Light mainLight;
        [SerializeField] private UniversalRenderPipelineAsset pc;
        [SerializeField] private UniversalRenderPipelineAsset mobile;

        [Tab("Panel")] [SerializeField] private Transform panel;

        [Tab("Pet")] [SerializeField] private TMP_InputField petInputField;
        [Tab("Trail")] [SerializeField] private TMP_InputField trailInputField;
        [Tab("FPS")] [SerializeField] private TMP_Text fpsText;


        private void Awake()
        {
            SetPanelState(false);
            enableButton.onClick.AddListener(() => SetPanelState(!panel.gameObject.activeSelf));
        }

        private void Update()
        {
            UpdateFPS();
        }

        private void SetPanelState(bool state)
        {
            var rotation = state ? Vector3.zero : new Vector3(0, 0, 180);
            panel.gameObject.SetActive(state);
            enableArrow.transform.eulerAngles = rotation;
        }

        public void Delete() => YG2.SetDefaultSaves();
        public void Save() => YG2.SaveProgress();
        public void Add1000FirstCurrency() => WalletManager.GetCurrency(CurrencyType.Crystal).Value += 1000;
        public void Add1000SecondCurrency() => WalletManager.GetCurrency(CurrencyType.SkillPoints).Value += 1;
        public void AddHeroPoints() => WalletManager.GetCurrency(CurrencyType.HeroPoints).Value += 1;
        public void AddLevel() => Player.Player.Instance.Stats.Experience +=
            Player.Player.Instance.Stats.ExperienceToLevelUp();

        private float _deltaTime;

        private void UpdateFPS()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            var fps = 1.0f / _deltaTime;
            fpsText.text = $"{fps:0.} FPS";
        }
    }
}