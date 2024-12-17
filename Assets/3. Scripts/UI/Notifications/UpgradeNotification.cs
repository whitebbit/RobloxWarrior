using System;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using TMPro;
using UnityEngine;

namespace _3._Scripts.UI.Notifications
{
    public class UpgradeNotification : UINotification
    {
        [SerializeField] private ScaleTransition transition;
        [SerializeField] private TMP_Text text;

        public override bool Condition => Player.Player.Instance.Stats.UpgradePoints > 0;

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;

            Player.Player.Instance.Stats.OnUpgradePointsChanged += OnUpgradePointsChanged;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            text.text = Player.Player.Instance.Stats.UpgradePoints.ToString();
        }

        private void OnUpgradePointsChanged(int obj)
        {
            text.text = obj.ToString();
        }
    }
}