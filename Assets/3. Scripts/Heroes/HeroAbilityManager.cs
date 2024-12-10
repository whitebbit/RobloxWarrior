using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Abilities.Enums;
using _3._Scripts.Abilities.Interfaces;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.Game;
using UnityEngine;

namespace _3._Scripts.Heroes
{
    public class HeroAbilityManager : MonoBehaviour
    {
        private readonly List<HeroAbility> _abilities = new();

        public void ClearAbilities()
        {
            foreach (var ability in _abilities)
            {
                ability.ResetAbility();
            }
            _abilities.Clear();
        }
        
        public void RegisterAbility(HeroAbility ability)
        {
            ability.ResetAbility();
            _abilities.Add(ability);
        }

        private void Update()
        {
            ActivateAbilities(AbilityTrigger.DuringBattle);
        }

        private void OnEnable()
        {
            GameEvents.OnWavePassed += ResetAbilities;
            GameEvents.OnStopBattle += ResetAbilities;
            GameEvents.OnBeforeBattle += () => ActivateAbilities(AbilityTrigger.BeforeBattle);
            GameEvents.OnWaveStart += () => ActivateAbilities(AbilityTrigger.OnWaveStart);
        }

        private void ResetAbilities()
        {
            foreach (var ability in _abilities)
            {
                ability.ResetAbility();
            }
        }

        private void ActivateAbilities(AbilityTrigger trigger)
        {
            foreach (var ability in _abilities.Where(a => a.Trigger == trigger))
            {
                ability.UseAbility();
            }
        }
    }
}