using System;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Currency
{
    public class CurrencyWidget : MonoBehaviour
    {
        [SerializeField] private CurrencyType type;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image icon;
        [SerializeField] private Image table;

        public CurrencyType Type => type;
        private CurrencyObject CurrencyObject => WalletManager.GetCurrency(Type);
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            var currency = Configuration.Instance.GetCurrency(type);
            icon.sprite = currency.Icon;

            OnChange(0, CurrencyObject.Value);
        }

        private void OnEnable()
        {
            CurrencyObject.OnValueChanged += OnChange;
            OnChange(0, CurrencyObject.Value);
        }

        private void OnDisable()
        {
            CurrencyObject.OnValueChanged -= OnChange;

        }

        private void OnChange(float oldValue, float newValue)
        {
            text.text = newValue.ConvertToWallet(10_000);
        }
    }
}