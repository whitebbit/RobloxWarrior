using _3._Scripts.UI.Interfaces;
using DG.Tweening;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI
{
    public abstract class UIPanel : UIElement
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            //transform.SetAsLastSibling();
        }

        protected override void OnClose()
        {
            base.OnClose();
            //transform.SetAsFirstSibling();
        }
    }
}