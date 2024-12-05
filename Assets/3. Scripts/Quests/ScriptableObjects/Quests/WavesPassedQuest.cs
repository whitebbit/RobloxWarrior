using _3._Scripts.Quests.Enums;
using UnityEngine;

namespace _3._Scripts.Quests.ScriptableObjects.Quests
{
    [CreateAssetMenu(fileName = "WavesPassedQuest", menuName = "Configs/Quests/Waves Passed Quest", order = 0)]
    public class WavesPassedQuest : Quest
    {
        [SerializeField] private int requiredWavesCount;

        public override QuestType Type => QuestType.WavesPassed;
        public override string GoalText => $"{requiredWavesCount}";
        public override bool IsCompleted => _currentWavesCount >= requiredWavesCount;
        public override string ProgressText => $"{_currentWavesCount}/{requiredWavesCount}";

        private int _currentWavesCount;

        protected override void UpdateProgress(object data)
        {
            if (data is not int waveCount) return;

            _currentWavesCount += waveCount;
            OnUpdateProgress();
        }

        protected override void ResetQuest()
        {
            _currentWavesCount = 0;
        }
    }
}