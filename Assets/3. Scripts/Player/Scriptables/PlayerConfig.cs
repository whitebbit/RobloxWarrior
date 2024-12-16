using System;
using System.Collections.Generic;
using _3._Scripts.Animations.Structs;
using Animancer;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

namespace _3._Scripts.Player.Scriptables
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player/Player Config", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [Tab("Base")] [SerializeField] private MovementConfig movementConfig = new();
        [SerializeField] private PlayerStatsConfig statsConfig = new();

        [Tab("Animations")] [SerializeField] private UnitAnimationConfig animationConfig = new();

        public MovementConfig MovementConfig => movementConfig;
        public UnitAnimationConfig AnimationConfig => animationConfig;
        public PlayerStatsConfig StatsConfig => statsConfig;
    }

    [Serializable]
    public class UnitAnimationConfig
    {
        [Header("Attack")] [SerializeField] private List<AttackAnimation> attackAnimations = new();
        [SerializeField] private AvatarMask attackMask;
        [Header("Movement")] [SerializeField] private LinearMixerTransition movementAnimation;
        [SerializeField] private AnimationClip jumpAnimation;
        [SerializeField] private AnimationClip fallAnimation;

        public AvatarMask AttackMask => attackMask;
        public LinearMixerTransition MovementAnimation => movementAnimation;
        public AnimationClip JumpAnimation => jumpAnimation;
        public AnimationClip FallAnimation => fallAnimation;
        public List<AttackAnimation> AttackAnimations => attackAnimations;
    }

    [Serializable]
    public class MovementConfig
    {
        [SerializeField] private float baseSpeed;
        [SerializeField] private float jumpHeight;

        public float BaseSpeed => baseSpeed;
        public float JumpHeight => jumpHeight;
    }

    [Serializable]
    public class PlayerStatsConfig
    {
        [Header("Experience")] 
        [SerializeField, Tooltip("Базовое количество опыта, необходимое для следующего уровня.")]
        private float baseExperience = 25f;

        [SerializeField, Tooltip("Множитель, который увеличивает базовый опыт.")]
        private float experienceMultiplier;

        [SerializeField, Tooltip("Начальное смещение опыта, добавляемое в зависимости от уровня.")]
        private float levelOffset;

        [SerializeField, Tooltip("Количество уровней, после которых увеличивается смещение.\n\n")]
        private float offsetIncrementFrequency;

        [SerializeField, Tooltip("Увеличение значения смещения через каждые offsetIncrementFrequency уровней.\n\n")]
        private float offsetIncrementValue;

        [Header("Stats Increase")] [SerializeField]
        private float healthImprovement;

        [SerializeField] private float attackImprovement;
        [SerializeField] private float speedImprovement;
        [SerializeField] private float critImprovement;

        [Header("Rebirth")] [SerializeField] private float rebirthRate;
        [Space] [SerializeField] private float attackPercentIncrease;
        [SerializeField] private float baseExperiencePercentIncrease;
        [Space] [SerializeField] private int rebirthStartLevelRequired = 5;

        [SerializeField, Tooltip("Количество уровней, которое добавляется после каждого перерождения")]
        private int rebirthIncreaseAmount = 5;

        [SerializeField, Tooltip("Каждое M-е перерождение увеличивает величину увеличения.")]
        private int rebirthIncreaseInterval = 3;

        public float BaseExperience => baseExperience;

        public float ExperienceMultiplier => experienceMultiplier;
        public float LevelOffset => levelOffset;
        public float OffsetIncrementValue => offsetIncrementValue;
        public float OffsetIncrementFrequency => offsetIncrementFrequency;

        public float HealthImprovement => healthImprovement;
        public float AttackImprovement => attackImprovement;
        public float SpeedImprovement => speedImprovement;
        public float CritImprovement => critImprovement;

        public float AttackPercentIncrease => attackPercentIncrease;
        public float BaseExperiencePercentIncrease => baseExperiencePercentIncrease;

        public int RebirthIncreaseAmount => rebirthIncreaseAmount;
        public int RebirthIncreaseInterval => rebirthIncreaseInterval;
        public int RebirthStartLevelRequired => rebirthStartLevelRequired;
    }
}