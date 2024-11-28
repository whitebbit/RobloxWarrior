using System.Collections.Generic;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Swords;
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
           
        public Transform SpawnPoint => spawnPoint;
        public void Initialize(WorldConfig config)
        {
            battleArena.Initialize(config);
            battleStarter.SetBattleArena(battleArena);
            
            Player.Player.Instance.Teleport(spawnPoint.position);

            for (int i = 0; i < config.SwordEggs.Count; i++)
            {
                swordUnlocks[i].Initialize(config.SwordEggs[i]);
            }
        }

        public void StopBattle()
        {
            battleArena.StopBattle();
        }
    }
}