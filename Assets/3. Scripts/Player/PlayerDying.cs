using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
using _3._Scripts.Units;
using _3._Scripts.Units.Interfaces;
using _3._Scripts.Worlds;

namespace _3._Scripts.Player
{
    public class PlayerDying : IDying
    {
        private UnitVFX _unitVFX;

        public PlayerDying(UnitVFX vfx = null)
        {
            _unitVFX = vfx;
        }

        public void SetVFX(UnitVFX vfx) => _unitVFX = vfx;
        
        public void Die()
        {
            _unitVFX?.OnDeath();
            UIManager.Instance.GetWidget<LoseWidget>().Enabled = true;
            WorldsManager.Instance.World.StopBattle();
        }

        public bool IsDead { get; set; }
    }
}