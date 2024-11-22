using _3._Scripts.Units.Interfaces;
using _3._Scripts.Worlds;

namespace _3._Scripts.Player
{
    public class PlayerDying : IDying
    {
        private readonly Player _player;
        public PlayerDying(Player player)
        {
            _player = player;
        }
        
        public void Die()
        {
            //TODO: UI
            
            WorldsManager.Instance.World.StopBattle();
            _player.Teleport(WorldsManager.Instance.World.SpawnPoint.position);
            _player.Health.Health = _player.Health.MaxHealth;
        }

        public bool IsDead { get; set; }
    }
}