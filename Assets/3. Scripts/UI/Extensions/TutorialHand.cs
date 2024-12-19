using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3._Scripts.UI.Extensions
{
    public class TutorialHand : MonoBehaviour
    {
        private Tween _tween;

        [SerializeField] private Vector3 startScale;

        private void OnValidate()
        {
            startScale = transform.localScale;
        }

        private void OnEnable()
        {
            transform.localScale = startScale;
            _tween = transform.DOScale(startScale * 1.25f, 0.35f).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            _tween.Kill();
        }
    }
}