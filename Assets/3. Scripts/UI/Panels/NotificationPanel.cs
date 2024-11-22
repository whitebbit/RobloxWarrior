using System;
using _3._Scripts.Localization;
using _3._Scripts.Singleton;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts.UI.Panels
{
    public class NotificationPanel : Singleton<NotificationPanel>
    {
        [SerializeField] private LocalizeStringEvent text;
        [SerializeField] private CanvasGroup panel;

        private Tween _currentTween;

        private void Start()
        {
            panel.alpha = 0;
        }

        public void ShowNotification(string messageId)
        {
            text.SetReference(messageId);
            DoAnimation();
        }

        private void DoAnimation()
        {
            if (_currentTween != null) return;

            _currentTween = panel.DOFade(1, 0.15f).OnComplete(() =>
            {
                panel.DOFade(0, 0.15f).SetDelay(1).OnComplete(() => _currentTween = null);
            });
        }
    }
}