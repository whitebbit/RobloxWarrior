using System;
using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Worlds.Scriptables
{
    [CreateAssetMenu(fileName = "WorldConfig", menuName = "Configs/World/World Config", order = 0)]
    public class WorldConfig : ScriptableObject
    {
        [SerializeField] private List<WaveData> waves = new();
        [SerializeField] private World worldPrefab;

        public World WorldPrefab => worldPrefab;
        public List<WaveData> Waves => waves;
        
    }
}