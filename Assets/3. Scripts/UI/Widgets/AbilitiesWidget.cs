using System.Collections.Generic;
using _3._Scripts.Abilities.Scriptables;
using _3._Scripts.UI.Elements;
using UnityEngine;
using YG;

namespace _3._Scripts.UI.Widgets
{
    public class AbilitiesWidget : MonoBehaviour
    {
        [SerializeField] private YG2.Device deviceType;
        [SerializeField] private List<AbilityWidget> widgets = new();

        private void Awake()
        {
            if (YG2.envir.device != deviceType)
                gameObject.SetActive(false);
        }

        private void Start()
        {
            if (YG2.envir.device  != deviceType) return;

            Player.Player.Instance.Ammunition.OnPlayerAbilitiesChanged.Subscribe(OnAbilitiesChanged);
        }

        private void OnAbilitiesChanged(List<PlayerAbility> obj)
        {
            for (int i = 0; i < widgets.Count; i++)
            {
                widgets[i].Locked = i >= YG2.saves.abilitiesSave.capacity;
                widgets[i].Initialize(i >= obj.Count ? null : obj[i]);
            }
        }
    }
}