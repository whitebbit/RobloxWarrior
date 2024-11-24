using System;
using _3._Scripts.UI.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.UI.Transitions
{
    [Serializable]
    public class SlideTransition : IUITransition
    {
        [SerializeField] private RectTransform transform;
        [Space] [SerializeField] private float duration;
        [SerializeField] private Vector2 direction;
        [SerializeField] private Ease inEase = Ease.Linear;
        [SerializeField] private Ease outEase = Ease.Linear;
        private Vector2 _startPosition;

        public void SetLinkTransition(IUITransition linkTransition) => LinkTransition = linkTransition;
        public IUITransition LinkTransition { get; set; }
        
        public void SetStartPosition()
        {
            _startPosition = transform.anchoredPosition;
        }
        
        public Tween AnimateIn()
        {
            ForceOut();
            LinkTransition?.ForceIn();
            return transform.DOAnchorPos(_startPosition, duration).SetLink(transform.gameObject).SetEase(inEase);
        }

        public Tween AnimateOut()
        {
            ForceIn();
            return transform.DOAnchorPos(_startPosition - direction, duration).SetLink(transform.gameObject)
                .SetEase(outEase);
        }

        public void ForceIn()
        {
            transform.anchoredPosition = _startPosition;
        }

        public void ForceOut()
        {
            transform.anchoredPosition = _startPosition - direction;
        }
    }
}