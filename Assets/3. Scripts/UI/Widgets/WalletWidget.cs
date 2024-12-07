using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using UnityEngine;

namespace _3._Scripts.UI.Widgets
{
    public class WalletWidget : UIWidget
    {
        [SerializeField] private SlideTransition transition;

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }
        public override void Initialize()
        {
            transition.SetStartPosition();

            InTransition = transition;
            OutTransition = transition;
        }

    }
}