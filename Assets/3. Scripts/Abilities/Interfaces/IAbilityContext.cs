using _3._Scripts.Units;
using UnityEngine;

namespace _3._Scripts.Abilities.Interfaces
{
    public interface IAbilityContext
    {
        public Unit Unit { get; }
        public UnitAnimator Animator { get; }
    }
}