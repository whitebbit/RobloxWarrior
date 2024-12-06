using System;
using _3._Scripts.Quests.Enums;
using UnityEngine;

namespace _3._Scripts.Quests.ScriptableObjects.Quests
{
    [CreateAssetMenu(fileName = "ClassicQuest", menuName = "Configs/Quests/Classic Quest", order = 0)]
    public class ClassicQuest : Quest
    {
        [SerializeField] private QuestType type;
        [SerializeField] private int requiredToComplete;

        private int _currentProgress;
        public override QuestType Type => type;
        public override string ProgressText => $"{_currentProgress}/{requiredToComplete}";
        public override int Goal => requiredToComplete;
        public override bool IsCompleted => _currentProgress >= requiredToComplete;

        private void OnValidate()
        {
            if (type != QuestType.CompleteWave) return;
            
            type = QuestType.WavesPassed;
            throw new ArgumentException("Этот тип не поддерживается данным классом", nameof(type));
        }

        protected override void UpdateProgress(object data)
        {
            if (data is int n)
            {
                _currentProgress += n;
                OnUpdateProgress();
            }

            if (data is "reset" && !IsCompleted)
            {
                ResetQuest();
                OnUpdateProgress();
            }
        }

        protected override void ResetQuest()
        {
            _currentProgress = 0;
        }
    }
}