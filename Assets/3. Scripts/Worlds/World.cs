using System.Collections.Generic;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Game;
using _3._Scripts.Quests;
using _3._Scripts.Quests.ScriptableObjects;
using _3._Scripts.Sounds;
using _3._Scripts.Swords;
using _3._Scripts.Tutorial;
using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
using _3._Scripts.Worlds.Scriptables;
using UnityEngine;
using UnityEngine.Serialization;
using YG;

namespace _3._Scripts.Worlds
{
    public class World : MonoBehaviour, IInitializable<WorldConfig>
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private BattleArena battleArena;
        [SerializeField] private BattleStarter battleStarter;
        [SerializeField] private List<SwordUnlocker> swordUnlocks = new();

        public List<Quest> Quests { get; private set; } = new();

        private WavesWidget Widget => UIManager.Instance.GetWidget<WavesWidget>();


        public void Initialize(WorldConfig config)
        {
            battleArena.Initialize(config);
            battleStarter.SetBattleArena(battleArena);

            Player.Player.Instance.Teleport(spawnPoint.position);

            for (var i = 0; i < config.SwordEggs.Count; i++)
            {
                swordUnlocks[i].Initialize(config.SwordEggs[i]);
            }

            Quests = config.Quests;

            AudioManager.Instance.PlaySound("lobby_music", loop: true);
        }

        public void StopBattle()
        {
            var player = Player.Player.Instance;

            battleArena.StopBattle();

            player.Teleport(spawnPoint.position);
            player.Health.Health = player.Health.MaxHealth;

            Widget.Enabled = false;

            GameEvents.StopBattle();
            YG2.SaveProgress();
            YG2.InterstitialAdvShow();

            AudioManager.Instance.StopLoop("battle_music");
            AudioManager.Instance.PlaySound("lobby_music", loop: true);

            if (Quester.CurrentQuest.IsCompleted)
                TutorialManager.Instance.StartStep("after_battle");
        }
    }
}