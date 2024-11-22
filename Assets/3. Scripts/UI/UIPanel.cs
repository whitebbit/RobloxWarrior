using _3._Scripts.UI.Interfaces;
using DG.Tweening;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI
{
    public abstract class UIPanel : UIElement
    {
        [Button]
        private void SwitchState()
        {
            Enabled = !Enabled;
        }
    }
}