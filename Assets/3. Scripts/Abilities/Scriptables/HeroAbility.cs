using System.Collections.Generic;
using _3._Scripts.Abilities.Enums;
using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Game;
using UnityEngine;

namespace _3._Scripts.Abilities.Scriptables
{
    public abstract class HeroAbility : ScriptableObject, ITriggeredAbility
    {
        [SerializeField] private string descriptionID;
        public abstract AbilityTrigger Trigger { get; }
        public virtual bool CanUse => GameContext.InBattle;
        public string DescriptionID => descriptionID;

        protected virtual Dictionary<string, object> DescriptionParameters()
        {
            return new Dictionary<string, object>
            {
                { "cooldown", 0f },
                { "value", 0f },
                { "duration", 0f }
            };
        }

        public T GetDescriptionParameters<T>(string valueName)
        {
            return (T)DescriptionParameters().GetValueOrDefault(valueName, default(T));
        }

        public void UseAbility(IAbilityContext context = null)
        {
            if (!CanUse) return;

            PerformAbility();
        }

        public abstract void ResetAbility();

        protected abstract void PerformAbility();
    }
}