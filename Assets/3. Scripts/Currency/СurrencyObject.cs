using System;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Saves;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace _3._Scripts.Currency
{
    public class CurrencyObject
    {
        private WalletSave Save => GBGames.saves.walletSave;
        public event Action<float, float> OnValueChanged;
        public readonly CurrencyType Type;

        public CurrencyObject(CurrencyType type)
        {
            Type = type;
        }

        public float Value
        {
            get => GetSaved(Type);
            set
            {
                if (Mathf.Approximately(GetSaved(Type), value)) return;

                var oldValue = GetSaved(Type);
                Change(Type, value);
                OnValueChanged?.Invoke(oldValue, value);
            }
        }

        public bool TrySpend(float amount)
        {
            if (!(Value >= amount)) return false;
            
            Value -= amount;
            return true;
        }


        private float GetSaved(CurrencyType currencyType)
        {
            return Type switch
            {
                CurrencyType.Crystal => Save.crystals,
                CurrencyType.HeroPoints => Save.heroPoints,
                CurrencyType.SkillPoints => Save.skillPoints,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void Change(CurrencyType currencyType, float value)
        {
            switch (currencyType)
            {
                case CurrencyType.Crystal:
                    Save.crystals = value;
                    break;
                case CurrencyType.HeroPoints:
                    Save.heroPoints = value;
                    break;
                case CurrencyType.SkillPoints:
                    Save.skillPoints = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null);
            }
        }
    }
}