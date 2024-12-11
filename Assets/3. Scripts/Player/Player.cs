using System;
using _3._Scripts.Quests.ScriptableObjects;
using _3._Scripts.UI;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Widgets;
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

        private PlayerStats _stats;
        public PlayerStats Stats => _stats ??= new PlayerStats();
        public PlayerAmmunition Ammunition { get; private set; }
        public override UnitHealth Health => _health ??= new UnitHealth(100, Dying);
        private PlayerDying Dying => _dying ??= new PlayerDying();

        private PlayerMovement _movement;
        private UnitHealth _health;
        private PlayerDying _dying;
        public PlayerController Controller { get; private set; }

        protected override void OnAwake()
        {
            base.OnAwake();
            _movement = GetComponent<PlayerMovement>();
            Ammunition = GetComponent<PlayerAmmunition>();
            Controller = GetComponent<PlayerController>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            Dying.SetVFX(VFX);
            Health.MaxHealth += Stats.HealthImprovement;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            Stats.OnLevelChange += _ => VFX.OnLevelUp();
            Stats.OnStatsChanged += () => Health.MaxHealth = Health.BaseHealth + Stats.HealthImprovement;
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