using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Bots;
using _3._Scripts.Bots.Sciptables;
using _3._Scripts.Config.Scriptables;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Currency.Scriptable;
using _3._Scripts.Game;
using _3._Scripts.Heroes.Scriptables;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Pool;
using _3._Scripts.Singleton;
using _3._Scripts.Swords.Scriptables;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Structs;
using _3._Scripts.Worlds.Scriptables;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Config
{
    public class Configuration : Singleton<Configuration>
    {
        [SerializeField] private MainConfig config;

        public MainConfig Config => config;

        public PlayerAbility GetAbility(string id) => config.Abilities.FirstOrDefault(a => a.ID == id);
        public CurrencyConfig GetCurrency(CurrencyType type) => config.CurrencyData.FirstOrDefault(c => c.Type == type);
        public RarityTable GetRarityTable(Rarity rarity) => config.RarityTables.FirstOrDefault(r => r.Rarity == rarity);

        public WorldConfig GetWorldConfig(string worldName) =>
            config.Worlds.FirstOrDefault(w => w.WorldName == worldName);

        public HeroConfig GetHero(string id) => config.Heroes.FirstOrDefault(h => h.ID == id);

        private void Start()
        {
            GBGames.GameReady();
            ResetContext();
        }

        private void ResetContext()
        {
            GameContext.StartWaveNumber = 1;
            GameContext.InBattle = false;
        }
    }
}