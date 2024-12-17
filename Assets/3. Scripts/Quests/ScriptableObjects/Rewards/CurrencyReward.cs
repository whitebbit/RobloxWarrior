using System;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
using UnityEngine;
using YG;

namespace _3._Scripts.Quests.ScriptableObjects.Rewards
{
    [CreateAssetMenu(fileName = "CurrencyReward", menuName = "Configs/Rewards/Currency", order = 0)]
    public class CurrencyReward : Reward
    {
        [SerializeField] private CurrencyType type;
        [SerializeField] private int amount;

        public override void GetReward()
        {
            float value;
            switch (type)
            {
                case CurrencyType.Crystal:
                    if (YG2.TryGetFlagAsFloat("crystal_booster", out var floatType))
                        value = amount * floatType;
                    else
                        value = amount;
                    break;
                case CurrencyType.HeroPoints:
                    value = amount;
                    break;
                case CurrencyType.SkillPoints:
                    value = amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UIManager.Instance.GetWidget<EffectsWidget>().Enabled = true;
            UIManager.Instance.GetWidget<EffectsWidget>()
                .ShowCurrency(type, value);
        }
    }
}