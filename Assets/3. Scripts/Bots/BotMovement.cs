using _3._Scripts.Bots.Sciptables;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Units;
using UnityEngine;

namespace _3._Scripts.Bots
{
    public sealed class BotMovement : UnitMovement<BotAnimator>, IInitializable<BotConfig>
    {
        private BotConfig _config;
        
        public void Initialize(BotConfig config)
        {
            _config = config;
        }

        private MovementConfig Config => _config.MovementConfig;
        protected override float Speed => Config.BaseSpeed;
    }
}