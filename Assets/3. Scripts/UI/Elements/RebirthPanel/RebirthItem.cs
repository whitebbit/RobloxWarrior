using _3._Scripts.Config;
using _3._Scripts.Currency;
using _3._Scripts.Localization;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts.UI.Elements.RebirthPanel
{
    public class RebirthItem : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent titleText;
        [SerializeField] private LocalizeStringEvent damageText;
        [SerializeField] private LocalizeStringEvent expText;

        public void Initialize(int rebirthLevel)
        {
            titleText.SetVariable("value", rebirthLevel.ToString());
            damageText.SetVariable("value",
                $"{rebirthLevel * Configuration.Instance.Config.PlayerConfig.StatsConfig.AttackPercentIncrease}");
            expText.SetVariable("value",
                $"{Player.Player.Instance.Stats.ExperienceIncrease(rebirthLevel).ConvertToWallet()}");
        }
    }
}