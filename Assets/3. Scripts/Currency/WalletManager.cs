using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Currency.Enums;

namespace _3._Scripts.Currency
{
    public static class WalletManager
    {
        private static readonly List<CurrencyObject> CurrencyObjects = new()
        {
            new CurrencyObject(CurrencyType.Crystal),
            new CurrencyObject(CurrencyType.HeroPoints),
            new CurrencyObject(CurrencyType.SkillPoints),
        };


        public static CurrencyObject GetCurrency(CurrencyType currencyType) =>
            CurrencyObjects.FirstOrDefault(c => c.Type == currencyType);

        private static string ConvertToWallet(this decimal number, decimal threshold = 1_000)
        {
            if (number < threshold)
            {
                return number.ToString("0"); // Формат без десятичных знаков
            }

            return number switch
            {
                < 1_000_000 => (number / 1_000m).ToString("0.#") + "K", // Формат без десятичных знаков
                < 1_000_000_000 => (number / 1_000_000m).ToString("0.#") + "M", // Формат без десятичных знаков
                < 1_000_000_000_000 => (number / 1_000_000_000m).ToString("0.#") + "B", // Формат без десятичных знаков
                < 1_000_000_000_000_000 => (number / 1_000_000_000_000m).ToString("0.#") +
                                           "T", // Формат без десятичных знаков
                < 1_000_000_000_000_000_000 => (number / 1_000_000_000_000_000m).ToString("0.#") +
                                               "Qa", // Формат без десятичных знаков
                < 1_000_000_000_000_000_000_000m => (number / 1_000_000_000_000_000_000m).ToString("0.#") +
                                                    "Qi", // Формат без десятичных знаков
                _ => (number / 1_000_000_000_000_000_000_000m).ToString("0.#") + "Sx" // Формат без десятичных знаков
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