using System;
using System.Collections.Generic;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Widgets
{
    public class AbilitiesWidget : MonoBehaviour
    {
        [SerializeField] private List<AbilityWidget> widgets = new();

        private void Start()
        {
            Player.Player.Instance.Ammunition.OnPlayerAbilitiesChanged.Subscribe(OnAbilitiesChanged);
        }

        private void OnAbilitiesChanged(List<PlayerAbility> obj)
        {
            for (int i = 0; i < widgets.Count; i++)
            {
                widgets[i].Locked = i >= GBGames.saves.abilitiesSave.capacity;
                widgets[i].Initialize(i >= obj.Count ? null : obj[i]);
            }
        }
    }
}