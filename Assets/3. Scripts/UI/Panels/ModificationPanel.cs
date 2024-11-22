using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Localization;
using _3._Scripts.UI.Elements.ModificationPanel;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class ModificationPanel : UIPanel
    {
        [SerializeField] private SlideTransition transition;

        [SerializeField] private ModificationItem prefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private LocalizeStringEvent scoreCountText;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button resetButton;
        [SerializeField] private TMP_Text resetPriceText;

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }

        private float ResetPrice()
        {
            return Player.Player.Instance.Stats.GetPointsSpent() * 50;
        }

        public override void Initialize()
        {
            transition.SetStartPosition();
            
            InTransition = transition;
            OutTransition = transition;
            
            inputField.onValueChanged.AddListener(ValidateInput);
            resetButton.onClick.AddListener(ResetStats);

            Player.Player.Instance.Stats.OnUpgradePointsChanged += StatsOnUpgradePointsChanged;

            StatsOnUpgradePointsChanged(Player.Player.Instance.Stats.UpgradePoints);
            InitializeItems();
        }

        private void StatsOnUpgradePointsChanged(int obj)
        {
            scoreCountText.SetVariable("value", obj);
            resetPriceText.text = $"{ResetPrice().ConvertToWallet(10_000)}<sprite index=1>";
        }

        private void ValidateInput(string input)
        {
            if (input.Length > 4)
            {
                inputField.text = input[..4];
            }

            if (IsValidNumber(input))
            {
                return;
            }

            inputField.text = RemoveInvalidCharacters(input);
        }

        private bool IsValidNumber(string input)
        {
            return float.TryParse(input, out _);
        }

        private string RemoveInvalidCharacters(string input)
        {
            return input.Where(c => char.IsDigit(c) || c == '.' || c == '-').Aggregate("", (current, c) => current + c);
        }

        private void InitializeItems()
        {
            foreach (var item in Configuration.Instance.Config.UIConfig.ModificationItems)
            {
                var obj = Instantiate(prefab, container);
                obj.Initialize(item);
                obj.SetInputField(inputField);
            }
        }

        private void ResetStats()
        {
            if (!WalletManager.TrySpend(CurrencyType.Crystal, ResetPrice())) return;

            Player.Player.Instance.Stats.ResetStats();
        }
    }
}