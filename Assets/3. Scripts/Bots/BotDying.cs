using _3._Scripts.Pool;
using _3._Scripts.Units;
using _3._Scripts.Units.Interfaces;
using UnityEngine;

namespace _3._Scripts.Bots
{
    public class BotDying : IDying
    {
        private readonly Bot _bot;
        private float _experience;
        private UnitVFX _vfx;
        public BotDying(Bot bot, UnitVFX vfx = null)
        {
            _bot = bot;
            _vfx = vfx;
        }

        public void SetExperience(float experience) => _experience = experience;
        public void SetVFX(UnitVFX vfx) => _vfx = vfx;

        public void Die()
        {
            if(IsDead) return;
            Player.Player.Instance.Stats.Experience += _experience;
            ObjectsPoolManager.Instance.Return(_bot);
            _vfx?.OnDeath();
            IsDead = true;
        }

        public bool IsDead { get; set; }
    }
}