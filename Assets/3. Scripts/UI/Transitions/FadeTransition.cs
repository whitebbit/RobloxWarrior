using System;
using _3._Scripts.UI.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.UI.Transitions
{
    [Serializable]
    public class FadeTransition : IUITransition
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [Space]
        [SerializeField] private float duration;
        [Space] [SerializeField] private Ease inEase;
        [SerializeField] private Ease outEase;
        
        public IUITransition LinkTransition { get; set; }

        public void SetLinkTransition(IUITransition linkTransition) => LinkTransition = linkTransition;
        
        public Tween AnimateIn()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 0;
            LinkTransition?.ForceIn();
            return canvasGroup.DOFade(1, duration).SetLink(canvasGroup.gameObject).SetEase(inEase);
        }

        public Tween AnimateOut()
        {
            canvasGroup.blocksRaycasts = false; 
            canvasGroup.alpha = 1;
            return canvasGroup.DOFade(0, duration).SetLink(canvasGroup.gameObject).SetEase(outEase);
        }

        public void ForceIn()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }

        public void ForceOut()
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }
}