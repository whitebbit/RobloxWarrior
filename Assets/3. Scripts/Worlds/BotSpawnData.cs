using System;
using _3._Scripts.Bots.Sciptables;
using UnityEngine;

namespace _3._Scripts.Worlds
{
    [Serializable]
    public class BotSpawnData
    {
        [SerializeField] private BotConfig config;
        [SerializeField] private int count;

        public BotConfig Config => config;
        public int Count => count;
    }
}