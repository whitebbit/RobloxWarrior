using System;
using _3._Scripts.UI.Enums;

namespace _3._Scripts.UI.Extensions
{
    public static class RarityExtensions
    {
        public static Rarity ToRarity(this int count)
        {
            return count switch
            {
                1 => Rarity.Common,
                2 => Rarity.Rare,
                3 => Rarity.Legendary,
                4 => Rarity.Immortal,
                5 => Rarity.Ancient,
                _ => throw new ArgumentOutOfRangeException(nameof(count), count, null)
            };
        }
    }
}