using System.Collections.Generic;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Quests.ScriptableObjects;
using _3._Scripts.Swords;
using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
using _3._Scripts.Worlds.Scriptables;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3._Scripts.Worlds
{
    public class World: MonoBehaviour, IInitializable<WorldConfig>
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private BattleArena battleArena;
        [SerializeField] private BattleStarter battleStarter; 
        [SerializeField] private List<SwordUnlocker> swordUnlocks = new();

        public List<Quest> Quests { get; private set; } = new();
        
        private Transform SpawnPoint => spawnPoint;
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
        }

        public void StopBattle()
        {
            var player = Player.Player.Instance;

            battleArena.StopBattle();
            
            player.Teleport(WorldsManager.Instance.World.SpawnPoint.position);
            player.Health.Health = player.Health.MaxHealth;

            Widget.Enabled = false;

            GameEvents.WaveFailed();
        }
        
        
    }
}