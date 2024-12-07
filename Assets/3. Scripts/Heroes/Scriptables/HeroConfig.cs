using System.Collections.Generic;
using _3._Scripts.Player.Scriptables;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Heroes.Scriptables
{
    [CreateAssetMenu(fileName = "HeroConfig", menuName = "Configs/Heroes/Hero Config", order = 0)]
    public class HeroConfig : ScriptableObject
    {
        [Tab("Base")] [SerializeField]
        private List<PassiveEffect> passiveEffects = new();

        
        [SerializeField] private MovementConfig movementConfig = new();
        
        [Tab("Model")]
        [SerializeField] private float size;
        [SerializeField] private Material skin;

        [Tab("Animation")] [SerializeField] private UnitAnimationConfig animationConfig = new();

        public List<PassiveEffect> PassiveEffects => passiveEffects;
        public Material Skin => skin;
        public MovementConfig MovementConfig => movementConfig;
        public UnitAnimationConfig AnimationConfig => animationConfig;
    }
}