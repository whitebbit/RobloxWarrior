using System;
using System.Collections.Generic;
using _3._Scripts.Swords.Scriptables;
using UnityEngine;

namespace _3._Scripts.Swords
{
    [Serializable]
    public class SwordUnlockerData
    {
        [SerializeField] private float price;
        [SerializeField] private Material eggMaterial;
        [SerializeField] private List<SwordConfig> swords = new();


        public float Price => price;
        public Material EggMaterial => eggMaterial;
        public List<SwordConfig> Swords => swords;
    }
}