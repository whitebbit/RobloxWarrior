using UnityEngine;

namespace _3._Scripts.Quests.ScriptableObjects
{
    public abstract class QuestGoal : ScriptableObject
    {
        public abstract bool Evaluate();
    }
}