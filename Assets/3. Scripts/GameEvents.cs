using System;
using _3._Scripts.Quests;
using _3._Scripts.Quests.Enums;
using _3._Scripts.Quests.ScriptableObjects;

namespace _3._Scripts
{
    public static class GameEvents
    {
        public static event Action OnEnemyKilled;

        public static void EnemyKilled()
        {
            OnEnemyKilled?.Invoke();
            QuestEventManager.Instance.RaiseEvent(QuestType.EnemyKills, 1);
        }

        public static event Action OnWavePassed;

        public static void WavePassed()
        {
            OnWavePassed?.Invoke();
            QuestEventManager.Instance.RaiseEvent(QuestType.WavesPassed, 1);
            
        }

        public static event Action<int> OnEggOpened;

        public static void EggOpened(int count)
        {
            OnEggOpened?.Invoke(count);
            QuestEventManager.Instance.RaiseEvent(QuestType.OpeningEgg, count);
        }

        public static event Action OnRebirth;

        public static void Rebirth()
        {
            OnRebirth?.Invoke();
            QuestEventManager.Instance.RaiseEvent(QuestType.Rebirth, 1);
        }

        public static event Action<int> OnWaveCompleted;
        public static void WaveCompleted(int waveNumber)
        {
            OnWaveCompleted?.Invoke(waveNumber);
            QuestEventManager.Instance.RaiseEvent(QuestType.CompleteWave, waveNumber);
        }

        public static event Action OnWaveFailed;
        public static void WaveFailed()
        {
            OnWaveFailed?.Invoke();
        }
        
        public static event Action<int> OnOpenEgg;
        public static void OpenEgg(int count)
        {
            OnOpenEgg?.Invoke(count);
            QuestEventManager.Instance.RaiseEvent(QuestType.OpeningEgg, count);

        }
    }
}