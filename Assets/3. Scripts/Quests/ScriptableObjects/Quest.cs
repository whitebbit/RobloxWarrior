using System;
using System.Collections.Generic;
using _3._Scripts.Quests.Enums;
using _3._Scripts.Quests.Interfaces;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Quests.ScriptableObjects
{
    public abstract class Quest : ScriptableObject, IQuestEvent
    {
        [SerializeField] private List<Reward> rewards;

        public event Action<Quest> OnProgressUpdate;
        public abstract QuestType Type { get; }
        public abstract string ProgressText { get; }
        public abstract int Goal { get; }
        public abstract bool IsCompleted { get; }

        public void Activate()
        {
            ResetQuest();
            QuestEventManager.Instance.RegisterListener(Type, this);
        }

        public void Deactivate()
        {
            OnProgressUpdate = null;
            QuestEventManager.Instance.UnregisterListener(Type, this);
        }

        public void GetRewards()
        {
            if (!IsCompleted) return;

            foreach (var reward in rewards)
            {
                reward.GetReward();
            }
        }

        protected abstract void UpdateProgress(object data);

        public void OnEventRaised(object sender, QuestEventArgs args)
        {
            if (args.EventType == Type)
            {
                UpdateProgress(args.EventData);
            }
        }

        protected void OnUpdateProgress()
        {
            OnProgressUpdate?.Invoke(this);
        }

        protected abstract void ResetQuest();
    }
}