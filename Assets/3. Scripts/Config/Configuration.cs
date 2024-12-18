using System.Linq;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Config.Scriptables;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Currency.Scriptable;
using _3._Scripts.Heroes.Scriptables;
using _3._Scripts.Singleton;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Structs;
using _3._Scripts.Worlds.Scriptables;
using UnityEngine;

namespace _3._Scripts.Config
{
    public class Configuration : Singleton<Configuration>
    {
        [SerializeField] private MainConfig config;

        public MainConfig Config => config;

        public PlayerAbility GetAbility(string id) => config.Abilities.FirstOrDefault(a => a.ID == id);

        public CurrencyConfig GetCurrency(CurrencyType type) =>
            config.UIConfig.CurrencyData.FirstOrDefault(c => c.Type == type);

        public RarityTable GetRarityTable(Rarity rarity) =>
            config.UIConfig.RarityTables.FirstOrDefault(r => r.Rarity == rarity);

        public WorldConfig GetWorldConfig(string worldName) =>
            config.Worlds.FirstOrDefault(w => w.WorldName == worldName);

        public HeroConfig GetHero(string id) => config.Heroes.FirstOrDefault(h => h.ID == id);
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }
    }
}