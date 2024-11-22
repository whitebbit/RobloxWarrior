using _3._Scripts.Config.Interfaces;
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
        
        public Transform SpawnPoint => spawnPoint;
        public void Initialize(WorldConfig config)
        {
            battleArena.Initialize(config);
            battleStarter.SetBattleArena(battleArena);
            
            Player.Player.Instance.Teleport(spawnPoint.position);
        }

        public void StopBattle()
        {
            battleArena.StopBattle();
        }
    }
}