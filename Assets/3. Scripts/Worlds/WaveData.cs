using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3._Scripts.Worlds
{
    [Serializable]
    public class WaveData
    {
        [SerializeField] private List<BotSpawnData> bots;
        [SerializeField] private float damageIncrease;
        [SerializeField] private float healthIncrease;
        
        public List<BotSpawnData> Bots => bots;

        public float DamageIncrease => damageIncrease;
        public float HealthIncrease => healthIncrease;
    }
}