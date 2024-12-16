using System.Collections.Generic;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Config.Scriptables;
using _3._Scripts.Player.Scriptables;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Heroes.Scriptables
{
    [CreateAssetMenu(fileName = "HeroConfig", menuName = "Configs/Heroes/Hero Config", order = 0)]
    public class HeroConfig : ConfigObject<Sprite>
    {
        [Tab("Base")] [SerializeField]
        private Sprite icon;

        
        [SerializeField] private HeroAbility ability;
        [SerializeField] private List<PassiveEffect> passiveEffects = new(2);

        [SerializeField] private MovementConfig movementConfig = new();

        [Tab("Model")] [SerializeField] private HeroModel modelPrefab;

        [Tab("Animation")] [SerializeField] private UnitAnimationConfig animationConfig = new();

        public List<PassiveEffect> PassiveEffects => passiveEffects;
        public HeroModel ModelPrefab => modelPrefab;
        public MovementConfig MovementConfig => movementConfig;
        public UnitAnimationConfig AnimationConfig => animationConfig;

        public HeroAbility Ability => ability;
        public override Sprite Icon => icon;
    }
}