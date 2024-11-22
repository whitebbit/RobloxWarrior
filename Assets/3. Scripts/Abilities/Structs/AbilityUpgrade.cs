using System;
using UnityEngine;

namespace _3._Scripts.Abilities.Structs
{
    [Serializable]
    public struct AbilityUpgrade : IEquatable<AbilityUpgrade>
    {
        public float priceToBreak;
        [Min(1)] public int maxLevel;

        public bool Equals(AbilityUpgrade other)
        {
            return priceToBreak.Equals(other.priceToBreak) && maxLevel == other.maxLevel;
        }

        public override bool Equals(object obj)
        {
            return obj is AbilityUpgrade other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(priceToBreak, maxLevel);
        }
    }
}