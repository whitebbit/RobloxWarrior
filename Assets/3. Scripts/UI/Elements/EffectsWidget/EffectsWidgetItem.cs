using System;
using _3._Scripts.Config;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Pool.Interfaces;
using _3._Scripts.UI.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements.EffectsWidget
{
    public class EffectsWidgetItem : MonoBehaviour, IPoolable
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text text;

        private Vector3 _startSize;

        private void Start()
        {
            _startSize = icon.transform.localScale;
        }

        public void Initialize(CurrencyType type, float amount)
        {
            var sprite = Configuration.Instance.GetCurrency(type).Icon;

            icon.sprite = sprite;
            icon.ScaleImage();

            text.text = $"+{amount.ConvertToWallet()}";
        }

        public void OnSpawn()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, 0.25f);
        }

        public void OnDespawn()
        {
            icon.transform.localScale = _startSize;
        }
    }
}