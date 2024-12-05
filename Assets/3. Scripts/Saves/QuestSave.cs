using System;
using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class QuestSave
    {
        public Dictionary<string, int> Quests = new();

        public int GetCurrentQuest(string world)
        {
            return Quests.TryAdd(world, 0) ? 0 : Quests[world];
        }

        public void SetNextQuest(string world)
        {
            if (!Quests.ContainsKey(world)) return;

            Quests[world] += 1;
        }
    }
}