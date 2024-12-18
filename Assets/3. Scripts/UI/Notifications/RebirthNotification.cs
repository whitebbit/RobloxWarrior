using _3._Scripts.Player;
using _3._Scripts.UI.Transitions;
using TMPro;
using UnityEngine;

namespace _3._Scripts.UI.Notifications
{
    public class RebirthNotification: UINotification
    {
        [SerializeField] private ScaleTransition transition;

        private PlayerStats Stats => Player.Player.Instance.Stats;
        public override bool Condition => Stats.Level >= Stats.LevelForRebirth;

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
        }
    }
}