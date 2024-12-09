using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Heroes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements.HeroPanel
{
    public class PassiveEffectItem : MonoBehaviour, IInitializable<PassiveEffect>
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text text;


        public void Initialize(PassiveEffect config)
        {
            var conf = Configuration.Instance.Config.UIConfig.ModificationItems.FirstOrDefault(i =>
                i.Type == config.Type);

            if (conf == null) return;

            icon.sprite = conf.Icon;
            text.text = $"+{config.Points}";
        }
    }
}