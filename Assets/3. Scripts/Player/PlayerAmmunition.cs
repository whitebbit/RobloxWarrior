using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Config;
using _3._Scripts.Extensions;
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

        private readonly List<PlayerAbility> _playerAbilities = new();


        private void Awake()
        {
            OnPlayerAbilitiesChanged = new LazyAction<List<PlayerAbility>>(_playerAbilities);
            var sword = new SwordSave(Configuration.Instance.Config.SwordCollectionConfig.Swords[0].ID);

            GBGames.saves.swordsSave.Unlock(sword);
            GBGames.saves.swordsSave.SetCurrent(
                GBGames.saves.swordsSave.unlocked.FirstOrDefault(s => s.uid == sword.uid));
            GBGames.saves.abilitiesSave.capacity = 2;
        }

        private void Start()
        {
            SetSword(GBGames.saves.swordsSave.current);
            SetAbilities(null);
        }

        public bool CanUseAbility() => _playerAbilities.All(a => a.Completed);

        public PlayerAbility GetPlayerAbility(int index) =>
            index >= _playerAbilities.Count ? null : _playerAbilities[index];

        private void SetSword(SwordSave save)
        {
            var config = Configuration.Instance.Config.SwordCollectionConfig.GetSword(save.id);

            if (Sword != null)
                Destroy(Sword.gameObject);

            Sword = Instantiate(config.Prefab, hand);
            Sword.transform.localPosition = localPosition;
            Sword.transform.localEulerAngles = localEulerAngles;

            Sword.SetSave(save);
            Sword.Initialize(config);
            Sword.SetOwner(Player.Instance.transform);
            Sword.SetStars(save.starCount);
        }

        public LazyAction<List<PlayerAbility>> OnPlayerAbilitiesChanged;

        private void SetAbilities(AbilitySave obj)
        {
            _playerAbilities.Clear();
            foreach (var config in GBGames.saves.abilitiesSave.selected.Select(ability =>
                         Configuration.Instance.GetAbility(ability.id)))
            {
                _playerAbilities.Add(config);
                config.ResetAbility();
            }

            OnPlayerAbilitiesChanged?.Invoke(_playerAbilities);
        }


        private void OnEnable()
        {
            GBGames.saves.swordsSave.OnSetCurrent += SetSword;
            GBGames.saves.abilitiesSave.OnSelect += SetAbilities;
        }

        private void OnDisable()
        {
            GBGames.saves.swordsSave.OnSetCurrent += SetSword;
        }
    }
}