using System;
using _3._Scripts.Currency.Enums;
using GBGamesPlugin;

namespace _3._Scripts.Currency
{
    public static class WalletManager
    {
        public static event Action<float, float> OnCrystalsChange;

        public static float Crystals
        {
            get => GBGames.saves.walletSave.crystals;
            set
            {
                GBGames.saves.walletSave.crystals = value;
                OnCrystalsChange?.Invoke(Crystals, value);
            }
        }

        public static event Action<float, float> OnHeroPointsChange;

        public static float HeroPoints
        {
            get => GBGames.saves.walletSave.heroPoints;
            set
            {
                GBGames.saves.walletSave.heroPoints = value;
                OnHeroPointsChange?.Invoke(HeroPoints, value);
            }
        }

        private static void SpendByType(CurrencyType currencyType, float count)
        {
            switch (currencyType)
            {
                case CurrencyType.Crystal:
                    Crystals -= count;
                    break;
                case CurrencyType.HeroPoints:
                    HeroPoints -= count;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null);
            }
        }

        public static void EarnByType(CurrencyType currencyType, float count)
        {
            switch (currencyType)
            {
                case CurrencyType.Crystal:
                    Crystals += count;
                    break;
                case CurrencyType.HeroPoints:
                    HeroPoints += count;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null);
            }
        }

        public static bool TrySpend(CurrencyType currencyType, float count)
        {
            var canSpend = GetQuantityByType(currencyType) >= count;

            if (canSpend)
                SpendByType(currencyType, count);

            return canSpend;
        }

        public static float GetQuantityByType(CurrencyType type)
        {
            return type switch
            {
                CurrencyType.Crystal => Crystals,
                CurrencyType.HeroPoints => HeroPoints,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private static string ConvertToWallet(this decimal number, decimal threshold = 1_000)
        {
            if (number < threshold)
            {
                return number.ToString("0");  // Формат без десятичных знаков
            }

            return number switch
            {
                < 1_000_000 => (number / 1_000m).ToString("0.#") + "K",  // Формат без десятичных знаков
                < 1_000_000_000 => (number / 1_000_000m).ToString("0.#") + "M",  // Формат без десятичных знаков
                < 1_000_000_000_000 => (number / 1_000_000_000m).ToString("0.#") + "B",  // Формат без десятичных знаков
                < 1_000_000_000_000_000 => (number / 1_000_000_000_000m).ToString("0.#") + "T",  // Формат без десятичных знаков
                < 1_000_000_000_000_000_000 => (number / 1_000_000_000_000_000m).ToString("0.#") + "Qa",  // Формат без десятичных знаков
                < 1_000_000_000_000_000_000_000m => (number / 1_000_000_000_000_000_000m).ToString("0.#") + "Qi",  // Формат без десятичных знаков
                _ => (number / 1_000_000_000_000_000_000_000m).ToString("0.#") + "Sx"  // Формат без десятичных знаков
            };
        }

        public static string ConvertToWallet(this float number, float threshold = 1_000)
        {
            return ((decimal)number).ConvertToWallet((decimal)threshold);
        }
        
        public static string ConvertToWallet(this int number, int threshold = 1_000)
        {
            return ((decimal)number).ConvertToWallet(threshold);
        }
    }
}