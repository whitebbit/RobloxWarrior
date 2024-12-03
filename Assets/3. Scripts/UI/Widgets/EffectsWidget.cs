using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Pool;
using _3._Scripts.UI.Elements.EffectsWidget;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.UI.Widgets
{
    public class EffectsWidget : UIWidget
    {
        [SerializeField] private FadeTransition transition;
        [Space] [SerializeField] private List<CurrencyWidget> widgets = new();
        [Space] [SerializeField] private float duration;
        [SerializeField] private Ease inEase;
        [SerializeField] private Ease outEase;

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
        }

        public void ShowCurrency(CurrencyType type, float amount)
        {
            var item = ObjectsPoolManager.Instance.Get<EffectsWidgetItem>();
            var widget = widgets.FirstOrDefault(i => i.Type == type);
            var itemRect = item.transform as RectTransform;

            item.transform.SetParent(transform);
            item.Initialize(type, amount);

            if (itemRect == null) return;

            itemRect.anchoredPosition = Vector3.zero + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);

            if (widget == null) return;
            
            var widgetRect = widget.transform as RectTransform;

            if (widgetRect == null) return;

            itemRect.DOMove(widgetRect.position, duration)
                .SetEase(inEase).SetEase(outEase)
                .OnComplete(() =>
                {
                    widget.transform.DOScale(1.1f, 0.15f)
                        .OnComplete(() => widget.transform.DOScale(1f, 0.15f));
                    
                    WalletManager.Crystals += amount;
                    ObjectsPoolManager.Instance.Return(item);
                    Enabled = false;
                });
        }
    }
}