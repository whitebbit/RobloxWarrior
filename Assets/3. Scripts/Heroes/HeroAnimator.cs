using _3._Scripts.Bots.Sciptables;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Heroes.Scriptables;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Units;

namespace _3._Scripts.Heroes
{
    public class HeroAnimator : UnitAnimator, IInitializable<HeroConfig>
    {
        protected override bool CanFall => !Grounded;
        
        public void Initialize(HeroConfig config)
        {
            Config = config.AnimationConfig;
        }
    }
}