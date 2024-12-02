using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Bots;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Pool;
using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
using _3._Scripts.Worlds.Scriptables;
using UnityEngine;

namespace _3._Scripts.Worlds
{
    public class BattleArena : MonoBehaviour, IInitializable<WorldConfig>
    {
        [SerializeField] private Transform playerPoint;
        [Space] [SerializeField] private BotSpawner spawner;

        private List<WaveData> _waves = new();
        private List<Bot> _bots = new();

        private int _waveIndex;

        private WavesWidget Widget => UIManager.Instance.GetWidget<WavesWidget>();

        public void Initialize(WorldConfig config)
        {
            _waves = config.Waves;
        }

        public void StartBattle()
        {
            var currentWave = _waves[_waveIndex];
            _bots = spawner.SpawnEnemies(currentWave);

            Player.Player.Instance.Teleport(playerPoint.position);

            if (!Widget.Enabled)
                Widget.Enabled = true;

            Widget.WaveNumber = _waveIndex + 1;

            StartCoroutine(CheckWaveEnd());
        }

        public void StopBattle()
        {
            StopAllCoroutines();

            _waveIndex = 0;
            foreach (var bot in _bots)
            {
                ObjectsPoolManager.Instance.Return(bot);
            }
        }

        private void WavePassed()
        {
            GameEvents.WavePassed();
            
            if (_waveIndex >= _waves.Count - 1)
            {
                
            }
            else
            {
                NextWave();
            }
        }

        private void NextWave()
        {
            _waveIndex++;
            StartBattle();
        }

        private IEnumerator CheckWaveEnd()
        {
            yield return new WaitUntil(() => _bots.All(b => b.Dying.IsDead));
            yield return new WaitForSeconds(2f);
            WavePassed();
        }
    }
}