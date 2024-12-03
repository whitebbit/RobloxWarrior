using _3._Scripts.Pool;
using _3._Scripts.Units.Interfaces;
using UnityEngine;

namespace _3._Scripts.Bots
{
    public class BotDying : IDying
    {
        private readonly Bot _bot;
        private float _experience;
        public BotDying(Bot bot)
        {
            _bot = bot;
        }

        public void SetExperience(float experience) => _experience = experience;
        
        public void Die()
        {
            if(IsDead) return;
            Player.Player.Instance.Stats.Experience += _experience;
            ObjectsPoolManager.Instance.Return(_bot);
            
            IsDead = true;
        }

        public bool IsDead { get; set; }
    }
}