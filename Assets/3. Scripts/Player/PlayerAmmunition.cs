using System;
using System.Linq;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Config;
using _3._Scripts.Saves;
using _3._Scripts.Swords;
using _3._Scripts.Swords.Scriptables;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class PlayerAmmunition : MonoBehaviour
    {
        [SerializeField] private Transform hand;
        [SerializeField] private Vector3 localPosition;
        [SerializeField] private Vector3 localEulerAngles;
        public Sword Sword { get; private set; }
        
        public Ability FirstAbility { get; private set; }
        public Ability SecondAbility { get; private set; }
        public Ability ThirdAbility { get; private set; }

        private void Awake()
        {
            var sword = new SwordSave(Configuration.Instance.Config.SwordCollectionConfig.Swords[0].ID);
            
            GBGames.saves.SwordsSave.Unlock(sword);
            GBGames.saves.SwordsSave.SetCurrent(GBGames.saves.SwordsSave.unlocked.FirstOrDefault(s => s.uid == sword.uid));

            SetSword(GBGames.saves.SwordsSave.current);
        }

        private void SetSword(SwordSave save)
        {
            var config = Configuration.Instance.Config.SwordCollectionConfig.GetSword(save.id);
            
            Sword = Instantiate(config.Prefab, hand);
            Sword.transform.localPosition = localPosition;
            Sword.transform.localEulerAngles = localEulerAngles;
            
            Sword.SetSave(save);
            Sword.Initialize(config);
            Sword.SetOwner(Player.Instance.transform);
        }
    }
}