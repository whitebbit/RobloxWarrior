using System.Collections.Generic;
using _3._Scripts.Currency.Scriptable;
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
        [Tab("Game Data")] [SerializeField] private List<WorldConfig> worlds = new();

        [Space] [SerializeField] private List<CurrencyConfig> currencyData = new();
        [SerializeField] private List<RarityTable> rarityTables = new();
        [Tab("Player Data")] [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private SwordCollectionConfig swordCollectionConfig;
        [Tab("UI Data")] [SerializeField] private UIConfig uiConfig;


        public PlayerConfig PlayerConfig => playerConfig;
        public SwordCollectionConfig SwordCollectionConfig => swordCollectionConfig;
        public List<CurrencyConfig> CurrencyData => currencyData;
        public List<RarityTable> RarityTables => rarityTables;
        public UIConfig UIConfig => uiConfig;

        public List<WorldConfig> Worlds => worlds;
    }
}