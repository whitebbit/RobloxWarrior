using System;
using _3._Scripts.Config;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Currency;
using _3._Scripts.Localization;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements.ModificationPanel
{
    public class ModificationItem : MonoBehaviour, IInitializable<ModificationItemConfig>
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private LocalizeStringEvent titleText;
        [SerializeField] private LocalizeStringEvent descriptionText;
        [SerializeField] private TMP_Text statsText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private LocalizeStringEvent levelText;

        private ModificationItemConfig _config;
        private TMP_InputField _inputField;
        private int ScoreAmount => int.TryParse(_inputField?.text, out var result) ? result : 1;
        
        public void Initialize(ModificationItemConfig config)
        {
            _config = config;
            iconImage.sprite = _config.Icon;
            iconImage.ScaleImage();

            titleText.SetReference(_config.TitleID);
            descriptionText.SetReference(_config.DescriptionID);

            UpdateStats();

            upgradeButton.onClick.AddListener(OnClick);

            Player.Player.Instance.Stats.OnStatsChanged += UpdateStats;
        }

        public void SetInputField(TMP_InputField inputField)
        {
            _inputField = inputField;
        }

        private string GetStats(ModificationType type)
        {
            var player = Player.Player.Instance;
            
            if (player == null) return "";
            if (player.Stats == null) return "";
            if (player.Ammunition == null) return "";
            if (player.Ammunition.Sword == null) return "";
            
            var baseStats = type switch
            {
                ModificationType.Health => "100",
                ModificationType.Attack =>
                    $"{player.GetTrueDamage(player.Ammunition.Sword.GetTrueDamage()).ConvertToWallet(10_000)}",
                ModificationType.Speed => $"{Configuration.Instance.Config.PlayerConfig.MovementConfig.BaseSpeed}",
                ModificationType.Crit => "0",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            var stats = type switch
            {
                ModificationType.Health => $"{player.Stats.HealthImprovement.ConvertToWallet()}" ,
                ModificationType.Attack => $"{player.Stats.AttackImprovement.ConvertToWallet()}",
                ModificationType.Speed => $"{player.Stats.SpeedImprovement.ConvertToWallet()}",
                ModificationType.Crit => $"{player.Stats.CritImprovement}",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            return $"{baseStats} (+{stats})";
        }

        public void UpdateStats()
        {
            if (_config == null) return;

            statsText.text = GetStats(_config.Type);
            levelText.SetVariable("value", Player.Player.Instance.Stats.GetLevel(_config.Type));
        }

        private void OnClick()
        {
            if (Player.Player.Instance.Stats.UpgradePoints <= 0) return;

            Player.Player.Instance.Stats.UpgradeStats(_config.Type, ScoreAmount);
            UpdateStats();
        }
    }
}