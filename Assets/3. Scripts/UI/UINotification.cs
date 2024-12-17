using System;
using _3._Scripts.UI.Interfaces;

namespace _3._Scripts.UI
{
    public abstract class UINotification : UIElement, INotification
    {
        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }

        public abstract bool Condition { get; }

    }
}