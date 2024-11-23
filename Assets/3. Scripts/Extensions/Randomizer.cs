using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Extensions.Interfaces;

namespace _3._Scripts.Extensions
{
    public static class Randomizer
    {
        public static T GetRandomElement<T>(this IEnumerable<T> elements) where T : IRandomable
        {
            var randomables = elements as T[] ?? elements.ToArray();
            
            if (randomables == null || !randomables.Any())
            {
                throw new ArgumentException("The list cannot be null or empty.", nameof(elements));
            }

            var totalChance = randomables.Sum(element => element.Chance);

            var randomValue = (float)new Random().NextDouble() * totalChance;

            var cumulativeChance = 0f;
            foreach (var element in randomables)
            {
                cumulativeChance += element.Chance;
                if (randomValue <= cumulativeChance)
                {
                    return element;
                }
            }

            return randomables[^1];
        }
    }
}