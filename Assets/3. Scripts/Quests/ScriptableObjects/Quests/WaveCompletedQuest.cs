using _3._Scripts.Quests.Enums;
using UnityEngine;

namespace _3._Scripts.Quests.ScriptableObjects.Quests
{
    [CreateAssetMenu(fileName = "WaveCompletedQuest", menuName = "Configs/Quests/Wave Completed Quest", order = 0)]
    public class WaveCompletedQuest : Quest
    {
        [SerializeField] private int waveNumber;

        private int _currentWave;
        private int Progress => _currentWave >= waveNumber ? 1 : 0;
        public override QuestType Type => QuestType.CompleteWave;
        public override string ProgressText => $"{Progress}/{1}";
        public override bool IsCompleted => _currentWave >= waveNumber;

        protected override void UpdateProgress(object data)
        {
            if (data is not int n) return;

            _currentWave = n;
            OnUpdateProgress();
        }

        protected override void ResetQuest()
        {
            _currentWave = 0;
        }
    }
}