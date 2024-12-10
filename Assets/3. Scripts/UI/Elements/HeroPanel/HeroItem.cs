using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Heroes.Scriptables;
using _3._Scripts.Localization;
using _3._Scripts.Swords.Scriptables;
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
                SetItemState(false);
                return;
            }

            SetItemState(true);
            Config = config;

            UpdateItem();
        }

        public void UpdateItem()
        {
            icon.texture = Config.Icon;
            var i = 0;

            foreach (var passiveEffect in passiveEffects)
            {
                passiveEffect.Initialize(Config.PassiveEffects[i]);
                i += 1;
            }

            if (!description) return;
            description.SetReference(Config.Ability.DescriptionID);

            description.SetVariable("value", Config.Ability.GetDescriptionParameters<float>("value"));
            description.SetVariable("cooldown", Config.Ability.GetDescriptionParameters<float>("cooldown"));
        }

        private void SetItemState(bool state)
        {
            icon.gameObject.SetActive(state);

            if (description)
                description.gameObject.SetActive(state);

            foreach (var passiveEffect in passiveEffects)
            {
                passiveEffect.gameObject.SetActive(state);
            }
        }
    }
}