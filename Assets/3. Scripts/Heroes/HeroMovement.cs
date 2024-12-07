using _3._Scripts.Config.Interfaces;
using _3._Scripts.Heroes.Scriptables;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Units;

namespace _3._Scripts.Heroes
{
    public class HeroMovement: UnitMovement<HeroAnimator>, IInitializable<HeroConfig>
    {
        private HeroConfig _config;
        private MovementConfig Config => _config.MovementConfig;
        protected override float Speed => Config.BaseSpeed;
        
        public void Initialize(HeroConfig config)
        {
            _config = config;
        }
    }
}