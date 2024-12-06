using System;
using _3._Scripts.Units.Interfaces;
using UnityEngine;

namespace _3._Scripts.Units
{
    public abstract class Unit : MonoBehaviour
    {

        public abstract UnitHealth Health { get; }
        public UnitDamageable Damageable { get; private set; }
        protected UnitVFX VFX;
        private void Awake()
        {
            OnAwake();
            VFX = GetComponent<UnitVFX>();
            Damageable = new UnitDamageable(Health, VFX);
        }

        private void Start()
        {
            OnStart();
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnAwake()
        {
        }
    }
}