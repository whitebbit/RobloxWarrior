using System;
using _3._Scripts.UI.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.UI.Transitions
{
    [Serializable]
    public class ScaleTransition : IUITransition
    {
        [SerializeField] private RectTransform transform;
        [Space] [SerializeField] private float inScale = 1;
        [SerializeField] private float outScale;
        [Space] [SerializeField] private float duration;
        [SerializeField] private Ease inEase = Ease.Linear;
        [SerializeField] private Ease outEase = Ease.Linear;

        public IUITransition LinkTransition { get; set; }

        public void SetLinkTransition(IUITransition transition)
        {
            LinkTransition = transition;
        }

        public Tween AnimateIn()
        {
            ForceOut();
            LinkTransition?.ForceIn();
            return transform.DOScale(inScale, duration).SetLink(transform.gameObject).SetEase(inEase);
        }

        public Tween AnimateOut()
        {
            ForceIn();
            return transform.DOScale(outScale, duration).SetLink(transform.gameObject).SetEase(outEase);
        }

        public void ForceIn()
        {
            transform.localScale = Vector3.one * inScale;
        }

        public void ForceOut()
        {
            transform.localScale = Vector3.zero;
        }
    }
}