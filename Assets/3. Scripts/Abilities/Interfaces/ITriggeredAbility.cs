using _3._Scripts.Abilities.Enums;

namespace _3._Scripts.Abilities.Interfaces
{
    public interface ITriggeredAbility: IAbility
    {
        AbilityTrigger Trigger { get; }
    }
}