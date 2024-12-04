using System;
using _3._Scripts.UI.Enums;
using UnityEngine;

namespace _3._Scripts.UI.Structs
{
    [Serializable]
    public struct RarityTable
    {
        [SerializeField] private Rarity rarity;
        [SerializeField] private Color mainColor;
        [SerializeField] private Color secondaryColor;
        [SerializeField] private string titleId;

        public Rarity Rarity => rarity;
        public string TitleID => titleId;
        public Color MainColor => mainColor;
        public Color SecondaryColor => secondaryColor;
        
    }
}