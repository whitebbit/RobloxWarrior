using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Bots;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Game;
using _3._Scripts.Pool;
using _3._Scripts.Sounds;
using _3._Scripts.UI;
using _3._Scripts.UI.Elements.EffectsWidget;
using _3._Scripts.UI.Widgets;
using _3._Scripts.Worlds.Scriptables;
using UnityEngine;
using YG;

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

        public void StartBattle(int waveNumber = -1)
        {
            _waveIndex = waveNumber > 0 ? waveNumber - 1 : _waveIndex;
            var currentWave = _waves[_waveIndex];

            _bots = spawner.SpawnEnemies(currentWave);
            Player.Player.Instance.Teleport(playerPoint.position);

            if (!Widget.Enabled)
                Widget.Enabled = true;

            Widget.WaveNumber = _waveIndex + 1;

            StartCoroutine(CheckWaveEnd());

            GameEvents.WaveStart();
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
            GameEvents.WaveCompleted(_waveIndex + 1);

            if (_waveIndex >= _waves.Count - 1)
            {
                WorldsManager.Instance.World.StopBattle();
            }
            else
            {
                NextWave();
            }
        }

        private void Reward()
        {
            AudioManager.Instance.PlaySound("wave_passed");
            UIManager.Instance.GetWidget<EffectsWidget>().Enabled = true;
            UIManager.Instance.GetWidget<EffectsWidget>()
                .ShowCurrency(CurrencyType.Crystal, _waves[_waveIndex].CrystalAmount);
        }

        private void NextWave()
        {
            _waveIndex++;
            StartBattle();
        }

        private IEnumerator CheckWaveEnd()
        {
            yield return new WaitUntil(() => _bots.All(b => b.Dying.IsDead));
            Reward();
            yield return new WaitForSeconds(2f);
            YG2.InterstitialAdvShow();
            WavePassed();
        }
    }
}