using _3._Scripts.Currency.Enums;
using UnityEngine;

namespace _3._Scripts.Currency.Scriptable
{
    [CreateAssetMenu(fileName = "CurrencyData", menuName = "Configs/Game/Currency Config", order = 0)]
    public class CurrencyConfig : ScriptableObject
    {
        [SerializeField] private CurrencyType type;
        [SerializeField] private Sprite icon;
        [SerializeField] private Color color;
        [SerializeField] private Color darkColor;

        public CurrencyType Type => type;
        public Color Color => color;
        public Color DarkColor => darkColor;
        public Sprite Icon => icon;
    }
}