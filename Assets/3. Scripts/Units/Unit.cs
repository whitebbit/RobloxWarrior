using System;
using _3._Scripts.Units.Interfaces;
using UnityEngine;

namespace _3._Scripts.Units
{
    public abstract class Unit : MonoBehaviour
    {
        public abstract UnitHealth Health { get; }
        public  UnitDamageable Damageable { get; protected set; }

        private void Awake()
        {
            OnAwake();
            Damageable = new UnitDamageable(Health);
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