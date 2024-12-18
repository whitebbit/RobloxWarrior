using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Transitions;
using TMPro;
using UnityEngine;

namespace _3._Scripts.UI.Notifications
{
    public class SkillsNotification: UINotification
    {
        [SerializeField] private ScaleTransition transition;
        [SerializeField] private TMP_Text text;

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;

            WalletManager.GetCurrency(CurrencyType.SkillPoints).OnValueChanged += OnUpgradePointsChanged;
        }

        public override bool Condition => WalletManager.GetCurrency(CurrencyType.SkillPoints).Value > 0;
        
        protected override void OnOpen()
        {
            base.OnOpen();
            text.text = WalletManager.GetCurrency(CurrencyType.SkillPoints).Value.ToString();
        }

        private void OnUpgradePointsChanged(float f, float f1)
        {
            text.text = f1.ToString();
        }
    }
}