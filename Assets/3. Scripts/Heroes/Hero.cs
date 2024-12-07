using System;
using _3._Scripts.Units;

namespace _3._Scripts.Heroes
{
    public class Hero : Unit
    {
        public override UnitHealth Health => _health;
        private UnitHealth _health;
        private HeroMovement _movement;
        
        protected override void OnAwake()
        {
            _health = new UnitHealth(10000, null);
            _movement = GetComponent<HeroMovement>();
        }

    }
}