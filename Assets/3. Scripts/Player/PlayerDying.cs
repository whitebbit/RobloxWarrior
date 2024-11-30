using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
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
            UIManager.Instance.GetWidget<LoseWidget>().Enabled = true;
            WorldsManager.Instance.World.StopBattle();
        }

        public bool IsDead { get; set; }
    }
}