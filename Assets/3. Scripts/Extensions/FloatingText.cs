using _3._Scripts.Pool;
using _3._Scripts.Pool.Interfaces;
using _3._Scripts.UI.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _3._Scripts.Extensions
{
    [RequireComponent(typeof(TMP_Text))]
    public class FloatingText : MonoBehaviour, IPoolable
    {
        [SerializeField] private TMP_Text text;

        public void Initialize(string value, Vector3 position)
        {
            var sequence = DOTween.Sequence();

            text.text = value;
            transform.position = position;
            transform.localScale = Vector3.zero;

            sequence.Append(text.transform.DOScale(1, .15f).SetEase(Ease.OutBack))
                .Append(text.transform.DOScale(0, .15f).SetDelay(0.25f).SetEase(Ease.InBack));

            sequence.Play().OnComplete(() => ObjectsPoolManager.Instance.Return(this));
        }

        public void SetColor(Color color) => text.color = color;
        public void SetGradient(VertexGradient gradient) => text.colorGradient = gradient;
        
        public void OnSpawn()
        {
            
        }
        public void OnDespawn()
        {
            
        }
    }
}