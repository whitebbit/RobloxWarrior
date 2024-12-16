using _3._Scripts.Currency.Enums;
using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
using UnityEngine;

namespace _3._Scripts.Quests.ScriptableObjects.Rewards
{
    [CreateAssetMenu(fileName = "CurrencyReward", menuName = "Configs/Rewards/Currency", order = 0)]
    public class CurrencyReward: Reward
    {
        [SerializeField] private CurrencyType type;
        [SerializeField] private int amount;
        
        public override void GetReward()
        {
            UIManager.Instance.GetWidget<EffectsWidget>().Enabled = true;
            UIManager.Instance.GetWidget<EffectsWidget>()
                .ShowCurrency(type, amount);
        }
    }
}