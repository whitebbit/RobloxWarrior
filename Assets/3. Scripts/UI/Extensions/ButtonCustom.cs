using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _3._Scripts.UI.Extensions
{
    public class ButtonCustom: Button
    {
        private RectTransform _rectTransform; 
        private Vector3 _originalScale;        
        public float scaleFactor = 1.1f;     

        protected override void Start()
        {
            base.Start();  
            _rectTransform = GetComponent<RectTransform>();
            _originalScale = _rectTransform.localScale;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            
            base.OnPointerEnter(eventData);
            _rectTransform.localScale = _originalScale * scaleFactor;
            _rectTransform.DOScale(_originalScale * scaleFactor, 0.1f);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _rectTransform.DOScale(_originalScale, 0.1f);
        }
    }
}