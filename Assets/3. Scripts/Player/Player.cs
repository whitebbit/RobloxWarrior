using _3._Scripts.Quests.ScriptableObjects;
using _3._Scripts.Units;
using _3._Scripts.Units.Interfaces;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class Player : Unit
    {
        #region Singleton

        public static Player Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var instances = FindObjectsOfType<Player>();
                var count = instances.Length;
                switch (count)
                {
                    case <= 0:
                        return _instance = new GameObject().AddComponent<Player>();
                    case 1:
                        return _instance = instances[0];
                }

                for (var i = 1; i < instances.Length; i++)
                    Destroy(instances[i]);

                return _instance = instances[0];
            }
        }

        private static Player _instance;

        #endregion
        
        public PlayerStats Stats { get; private set; }
        public PlayerAmmunition Ammunition { get; private set; }
        public override UnitHealth Health => _health;

        private PlayerMovement _movement;
        private UnitHealth _health;

        protected override void OnAwake()
        {
            base.OnAwake();
            _health = new UnitHealth(100, new PlayerDying(this));
            _movement = GetComponent<PlayerMovement>();
            Stats = new PlayerStats();
            Ammunition = GetComponent<PlayerAmmunition>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            Health.MaxHealth += Stats.HealthImprovement;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            Stats.OnHealthImproving += () => Health.MaxHealth += Stats.HealthImprovement;
        }

        public float GetTrueDamage(float swordDamage)
        {
            var increasePercent = Stats.AttackPercentIncrease / 100;
            var increaseDamage = swordDamage * increasePercent;
            return swordDamage + increaseDamage;
        }

        public float GetDamage(float swordDamage)
        {
            return GetTrueDamage(swordDamage) + Stats.AttackImprovement;
        }

        public void Teleport(Vector3 position)
        {
            _movement.Teleport(position);
            transform.eulerAngles = Vector3.zero;
            CameraController.Instance.OnTeleport();
        }
    }
}