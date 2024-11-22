using System;
using System.Collections.Generic;
using _3._Scripts.UI.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.UI.Transitions
{
    [Serializable]
    public class ManyObjectsSlideTransition : IUITransition
    {
        [SerializeField] private List<RectTransform> transforms = new();
        [Space] [SerializeField] private float duration;
        [SerializeField] private Vector2 direction;

        public void SetLinkTransition(IUITransition transition)
        {
            throw new NotImplementedException();
        }

        public IUITransition LinkTransition { get; set; }

        public Tween AnimateIn()
        {
            for (var i = 0; i < transforms.Count - 1; i++)
            {
                transforms[i].DOAnchorPos(transforms[i].anchoredPosition + direction, duration);
            }

            return transforms[^1].DOAnchorPos(transforms[^1].anchoredPosition + direction, duration);
        }

        public Tween AnimateOut()
        {
            for (var i = 0; i < transforms.Count - 1; i++)
            {
                transforms[i].DOAnchorPos(transforms[i].anchoredPosition - direction, duration);
            }

            return transforms[^1].DOAnchorPos(transforms[^1].anchoredPosition - direction, duration);
        }

        public void ForceIn()
        {
            foreach (var t in transforms)
            {
                t.anchoredPosition += direction;
            }
        }

        public void ForceOut()
        {
            foreach (var t in transforms)
            {
                t.anchoredPosition -= direction;
            }
        }
    }
}