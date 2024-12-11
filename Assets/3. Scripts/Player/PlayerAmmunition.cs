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
        [SerializeField] private PlayerAbility ability;
        
        public Sword Sword { get; private set; }

        public PlayerAbility FirstPlayerAbility => ability;
        public PlayerAbility SecondPlayerAbility { get; private set; }
        public PlayerAbility ThirdPlayerAbility { get; private set; }

        private void Awake()
        {
            var sword = new SwordSave(Configuration.Instance.Config.SwordCollectionConfig.Swords[0].ID);
            
            GBGames.saves.swordsSave.Unlock(sword);
            GBGames.saves.swordsSave.SetCurrent(GBGames.saves.swordsSave.unlocked.FirstOrDefault(s => s.uid == sword.uid));

            SetSword(GBGames.saves.swordsSave.current);
        }

        private void Start()
        {
            ability.ResetAbility();
        }

        private void SetSword(SwordSave save)
        {
            var config = Configuration.Instance.Config.SwordCollectionConfig.GetSword(save.id);
            
            if(Sword != null)
                Destroy(Sword.gameObject);
            
            Sword = Instantiate(config.Prefab, hand);
            Sword.transform.localPosition = localPosition;
            Sword.transform.localEulerAngles = localEulerAngles;
            
            Sword.SetSave(save);
            Sword.Initialize(config);
            Sword.SetOwner(Player.Instance.transform);
            Sword.SetStars(save.starCount);
        }

        private void OnEnable()
        {
            GBGames.saves.swordsSave.OnSetCurrent += SetSword;
        }

        private void OnDisable()
        {
            GBGames.saves.swordsSave.OnSetCurrent += SetSword;
        }
    }
}