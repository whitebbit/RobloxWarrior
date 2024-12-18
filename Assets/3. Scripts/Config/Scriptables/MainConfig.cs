using System.Collections.Generic;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Currency.Scriptable;
using _3._Scripts.Extensions.Scriptables;
using _3._Scripts.Heroes.Scriptables;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.UI.Scriptables;
using _3._Scripts.UI.Structs;
using _3._Scripts.Worlds.Scriptables;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Config.Scriptables
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "Configs/Main Config", order = 0)]
    public class MainConfig : ScriptableObject
    {
        [Tab("Game Data")] 
        [SerializeField] private List<WorldConfig> worlds = new();
        [SerializeField] private List<HeroConfig> heroes = new();
        [Header("Qualities")] 
        [SerializeField] private List<QualityConfig> qualityConfigs = new();

        
        [Tab("Player Data")] 
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private SwordCollectionConfig swordCollectionConfig;
        [SerializeField] private List<PlayerAbility> abilities = new();
        [SerializeField] private List<PlayerClass> classes = new();
        
        [Tab("UI Data")] 
        [SerializeField] private UIConfig uiConfig;
        
        public PlayerConfig PlayerConfig => playerConfig;
        public SwordCollectionConfig SwordCollectionConfig => swordCollectionConfig;
        public UIConfig UIConfig => uiConfig;
        public List<WorldConfig> Worlds => worlds;
        public List<HeroConfig> Heroes => heroes;
        public List<QualityConfig> QualityConfigs => qualityConfigs;
        public List<PlayerAbility> Abilities => abilities;

        public List<PlayerClass> Classes => classes;
    }
}