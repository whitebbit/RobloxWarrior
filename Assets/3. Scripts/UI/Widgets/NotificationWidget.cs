using System;
using System.Collections;
using _3._Scripts.Localization;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts.UI.Widgets
{
    public class NotificationWidget : UIWidget
    {
        [SerializeField] private FadeTransition transition;
        [Space] [SerializeField] private LocalizeStringEvent text;

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            StartCoroutine(DelayDisable());
        }

        private void OnEnable()
        {
            if (Enabled)
                Enabled = false;
        }

        public void SetText(string localizeID) => text.SetReference(localizeID);

        private IEnumerator DelayDisable()
        {
            yield return new WaitForSeconds(1f);
            Enabled = false;
        }
    }
}