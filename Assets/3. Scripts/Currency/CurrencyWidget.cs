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
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            var currency = Configuration.Instance.GetCurrency(type);
            icon.sprite = currency.Icon;
            //table.color = currency.DarkColor;
            //icon.ScaleImage();

            switch (type)
            {
                case CurrencyType.Crystal:
                    OnChange(0, WalletManager.Crystals);
                    break;
                case CurrencyType.HeroPoints:
                    OnChange(0, WalletManager.HeroPoints);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnEnable()
        {
            switch (type)
            {
                case CurrencyType.Crystal:
                    WalletManager.OnCrystalsChange += OnChange;
                    OnChange(0, WalletManager.Crystals);
                    break;
                case CurrencyType.HeroPoints:
                    WalletManager.OnHeroPointsChange += OnChange;
                    OnChange(0, WalletManager.HeroPoints);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDisable()
        {
            switch (type)
            {
                case CurrencyType.Crystal:
                    WalletManager.OnCrystalsChange -= OnChange;
                    break;
                case CurrencyType.HeroPoints:
                    WalletManager.OnHeroPointsChange -= OnChange;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnChange(float _, float newValue)
        {
            text.text = newValue.ConvertToWallet(10_000);
        }
    }
}