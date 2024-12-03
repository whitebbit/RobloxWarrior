﻿using _3._Scripts.Player.Scriptables;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Bots.Sciptables
{
    [CreateAssetMenu(fileName = "BotConfig", menuName = "Configs/Bots/Bot Config", order = 0)]
    public class BotConfig : ScriptableObject
    {
        [Tab("Base")] 
        [SerializeField] private float health;
        [Space] 
        [SerializeField] private float damage;
        [SerializeField] private float attackSpeed;

        [Space] 
        [SerializeField] private MovementConfig movementConfig = new();
        [Space] [SerializeField] private float experience;
        
        [Tab("Model")]
        [SerializeField] private float size;
        [SerializeField] private Material skin;

        [Tab("Animation")] [SerializeField] private UnitAnimationConfig animationConfig = new();


        public Material Skin => skin;
        public MovementConfig MovementConfig => movementConfig;
        public UnitAnimationConfig AnimationConfig => animationConfig;

        public float Size => size;
        public float Health => health;

        public float Damage => damage;
        public float AttackSpeed => attackSpeed;

        public float Experience => experience;
    }
}