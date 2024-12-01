using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _3._Scripts.Quests.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Configs/Quests/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] private string descriptionID;
        [SerializeField] private List<QuestGoal> goals;
        [SerializeField] private List<Reward> rewards;

        public bool IsCompleted()
        {
            return goals.All(goal => goal.Evaluate());
        }

        public void GetRewards()
        {
            foreach (var reward in rewards)
            {
                reward.GetReward();
            }
        }
    }
}