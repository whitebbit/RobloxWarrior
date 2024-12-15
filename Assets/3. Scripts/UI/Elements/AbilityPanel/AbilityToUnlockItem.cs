using System;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Localization;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements.AbilityPanel
{
    [Serializable]
    public class AbilityToUnlockItem: MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        
        [SerializeField] private LocalizeStringEvent abilityToUnlockText;
        [SerializeField] private LocalizeStringEvent abilityLevelText;

        public void Initialize(Sprite icon,string abilityId, int level)
        {
            iconImage.sprite = icon;
            abilityToUnlockText.SetReference(abilityId);
            abilityLevelText.SetVariable("value", level);
        }
    }
}