using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Extensions.Interfaces;

namespace _3._Scripts.Extensions
{
    public static class Randomizer
    {
        public static T GetRandomElement<T>(this IEnumerable<T> elements, RandomType type = RandomType.Default)
            where T : IRandomable
        {
            var randomables = elements as T[] ?? elements.ToArray();

            if (randomables == null || !randomables.Any())
            {
                throw new ArgumentException("The list cannot be null or empty.", nameof(elements));
            }

            float randomValue;
            var cumulativeChance = 0f;
            
            switch (type)
            {
                case RandomType.Default:
                    var totalChance = randomables.Sum(element => element.Chance);

                    randomValue = (float)new Random().NextDouble() * totalChance;

                    foreach (var element in randomables)
                    {
                        cumulativeChance += element.Chance;
                        if (randomValue <= cumulativeChance)
                        {
                            return element;
                        }
                    }

                    return randomables[^1];
                case RandomType.Normalize:
                    var minChance = randomables.Min(element => element.Chance);
                    var scaledChances = randomables
                        .Select(element => (Original: element, ScaledChance: element.Chance / minChance))
                        .ToArray();

                    var totalScaledChance = scaledChances.Sum(x => x.ScaledChance);

                    randomValue = (float)new Random().NextDouble() * totalScaledChance;

                    foreach (var (original, scaledChance) in scaledChances)
                    {
                        cumulativeChance += scaledChance;
                        if (randomValue <= cumulativeChance)
                        {
                            return original;
                        }
                    }

                    return scaledChances[^1].Original;
                case RandomType.Logarithmic:
                    const float offset = 1f; 
                    var transformedChances1 = randomables
                        .Select(element => (Original: element, TransformedChance: MathF.Log(element.Chance + offset)))
                        .ToArray();
                    var totalTransformedChance1 = transformedChances1.Sum(x => x.TransformedChance);
                    
                    randomValue = (float)new Random().NextDouble() * totalTransformedChance1;

                    foreach (var (original, transformedChance) in transformedChances1)
                    {
                        cumulativeChance += transformedChance;
                        if (randomValue <= cumulativeChance)
                        {
                            return original;
                        }
                    }

                    return transformedChances1[^1].Original;
                case RandomType.Exponential:

                    const float scaleFactor = 2f;
                    var transformedChances = randomables
                        .Select(element =>
                            (Original: element, TransformedChance: MathF.Pow(element.Chance, scaleFactor)))
                        .ToArray();

                    var totalTransformedChance = transformedChances.Sum(x => x.TransformedChance);

                    randomValue = (float)new Random().NextDouble() * totalTransformedChance;

                    foreach (var (original, transformedChance) in transformedChances)
                    {
                        cumulativeChance += transformedChance;
                        if (randomValue <= cumulativeChance)
                        {
                            return original;
                        }
                    }

                    return transformedChances[^1].Original;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        public static bool GetRandom(this float chance)
        {
            return UnityEngine.Random.Range(0f, 100f) < chance;
        }
    }

    public enum RandomType
    {
        Default,
        Normalize,
        Logarithmic,
        Exponential
    }
}