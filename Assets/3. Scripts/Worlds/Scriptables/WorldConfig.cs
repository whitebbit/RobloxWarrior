﻿using System;
using System.Collections.Generic;
using _3._Scripts.Swords;
using UnityEngine;

namespace _3._Scripts.Worlds.Scriptables
{
    [CreateAssetMenu(fileName = "WorldConfig", menuName = "Configs/World/World Config", order = 0)]
    public class WorldConfig : ScriptableObject
    {
        [SerializeField] private List<WaveData> waves = new();
        [SerializeField] private World worldPrefab;
        [SerializeField] private List<SwordUnlockerData> swordEggs = new();
        
        public World WorldPrefab => worldPrefab;
        public List<WaveData> Waves => waves;
        public List<SwordUnlockerData> SwordEggs => swordEggs;
    }
}