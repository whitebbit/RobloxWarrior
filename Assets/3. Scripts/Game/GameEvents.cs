using System;
using _3._Scripts.Quests;
using _3._Scripts.Quests.Enums;

namespace _3._Scripts.Game
{
    public static class GameEvents
    {
        public static event Action OnEnemyKilled;

        public static void EnemyKilled()
        {
            OnEnemyKilled?.Invoke();
            QuestEventManager.Instance.RaiseEvent(QuestType.EnemyKills, 1);
        }

        public static event Action OnRebirth;

        public static void Rebirth()
        {
            OnRebirth?.Invoke();
            QuestEventManager.Instance.RaiseEvent(QuestType.Rebirth, 1);
        }

        #region Waves

        public static event Action OnWaveStart;

        public static void WaveStart()
        {
            OnWaveStart?.Invoke();
        }

        public static event Action<int> OnWaveCompleted;

        public static void WaveCompleted(int waveNumber)
        {
            OnWaveCompleted?.Invoke(waveNumber);
            QuestEventManager.Instance.RaiseEvent(QuestType.CompleteWave, waveNumber);
        }

        public static event Action OnWavePassed;

        public static void WavePassed()
        {
            OnWavePassed?.Invoke();
            QuestEventManager.Instance.RaiseEvent(QuestType.WavesPassed, 1);
        }

        public static event Action OnWaveFailed;

        public static void WaveFailed()
        {
            OnWaveFailed?.Invoke();
        }

        #endregion

        #region Battle

        public static event Action OnBeforeBattle;

        public static void BeforeBattle()
        {
            GameContext.InBattle = true;
            OnBeforeBattle?.Invoke();
        }

        public static event Action OnStopBattle;

        public static void StopBattle()
        {
            OnStopBattle?.Invoke();
            GameContext.InBattle = false;
        }

        #endregion

        public static event Action<int> OnOpenEgg;

        public static void OpenEgg(int count)
        {
            OnOpenEgg?.Invoke(count);
            QuestEventManager.Instance.RaiseEvent(QuestType.OpeningEgg, count);
        }

        public static event Action OnSkillUpgrade;

        public static void SkillUpgrade()
        {
            OnSkillUpgrade?.Invoke();
            QuestEventManager.Instance.RaiseEvent(QuestType.SkillUpgrade, 1);
        }
        
        public static event Action OnGetHero;

        public static void GetHero()
        {
            OnGetHero?.Invoke();
            QuestEventManager.Instance.RaiseEvent(QuestType.GetHero, 1);
        }
    }
}