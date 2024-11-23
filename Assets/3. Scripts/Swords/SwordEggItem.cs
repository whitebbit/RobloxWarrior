using _3._Scripts.Config;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Swords.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Swords
{
    public class SwordEggItem : MonoBehaviour, IInitializable<SwordConfig>
    {
        [SerializeField] private RawImage icon;
        [SerializeField] private TMP_Text chanceText;
        [SerializeField] private Image table;
        
        public void Initialize(SwordConfig config)
        {
            var rarity = Configuration.Instance.GetRarityTable(config.Rarity);
            
            table.color = rarity.MainColor;
            chanceText.text = $"{config.Chance}%";
            icon.texture = config.Icon;
        }
    }
}