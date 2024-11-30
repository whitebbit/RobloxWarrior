using System.Collections;
using System.Collections.Generic;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using UnityEngine;

namespace _3._Scripts.UI.Widgets
{
    public class LoseWidget : UIWidget
    {
        [SerializeField] private FadeTransition transition;
        [SerializeField] private float timeToHide = 2;
        
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


        private IEnumerator DelayDisable()
        {
            yield return new WaitForSeconds(timeToHide);
            Enabled = false;
        }
    }
}