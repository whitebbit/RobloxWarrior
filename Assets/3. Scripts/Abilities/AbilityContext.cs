using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Units;

namespace _3._Scripts.Abilities
{
    public class AbilityContext: IAbilityContext
    {
        public AbilityContext(Unit unit, UnitAnimator animator)
        {
            Unit = unit;
            Animator = animator;
        }

        public Unit Unit { get; }
        public UnitAnimator Animator { get; }
    }
}