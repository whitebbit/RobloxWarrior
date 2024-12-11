using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Heroes;
using _3._Scripts.Pool;
using _3._Scripts.Saves;
using _3._Scripts.Saves.Handlers;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class PlayerHeroManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> heroPoint;

        private HeroesSave Save => GBGames.saves.heroesSave;
        private readonly List<Hero> _heroes = new();

        private void OnEnable()
        {
            Save.OnSelect += SaveOnOnSelect;
        }
        private void OnDisable()
        {
            Save.OnSelect -= SaveOnOnSelect;
        }
        private void SaveOnOnSelect(HeroSave obj)
        {
            ClearHeroes();
            SpawnHeroes();
        }

        private void Start()
        {
            SpawnHeroes();
        }

        private void ClearHeroes()
        {
            foreach (var hero in _heroes)
            {
                ObjectsPoolManager.Instance.Return(hero);
            }
        }

        private void SpawnHeroes()
        {
            for (var i = 0; i < Save.selected.Count; i++)
            {
                var item = Save.selected[i];
                var point = heroPoint[i];
                var config = Configuration.Instance.Config.Heroes.FirstOrDefault(h => h.ID == item.id);

                var hero = ObjectsPoolManager.Instance.Get<Hero>();
                hero.Initialize(config);
                hero.transform.position = point.position;

                hero.SetTarget(point);
                _heroes.Add(hero);
            }
        }
    }
}