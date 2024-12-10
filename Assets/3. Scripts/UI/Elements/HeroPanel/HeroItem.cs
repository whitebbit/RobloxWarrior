using System.Collections.Generic;
using _3._Scripts.Heroes.Scriptables;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;

namespace _3._Scripts.UI.Elements.HeroPanel
{
    public class HeroItem : CollectionItem<HeroConfig, HeroItem>
    {
        [SerializeField] private LocalizeStringEvent description;
        [SerializeField] private List<PassiveEffectItem> passiveEffects = new();
        protected override HeroItem Self => this;
        public override void Initialize(HeroConfig config)
        {
            if (config == null)
            {
                icon.gameObject.SetActive(false);
                description.gameObject.SetActive(false);
                foreach (var passiveEffect in passiveEffects)
                {
                    passiveEffect.gameObject.SetActive(false);
                }
                return;
            }

            Config = config;
            
            table.color = rarity.MainColor;
            damageText.text = $"{SwordDamage}";
            icon.texture = config.Icon;

            // Обновление звезд
            for (var i = 0; i < stars.Count; i++)
            {
                stars[i].gameObject.SetActive(i < save.starCount);
            }
        }
    }
}