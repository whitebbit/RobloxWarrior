using System.Collections.Generic;
using _3._Scripts.Currency.Scriptable;
using _3._Scripts.UI.Structs;
using UnityEngine;

namespace _3._Scripts.UI.Scriptables
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Configs/UI/UI Config",
        order = 0)]    
    public class UIConfig : ScriptableObject
    {
        [SerializeField] private List<ModificationItemConfig> modificationItems = new();
        [SerializeField] private List<CurrencyConfig> currencyData = new();
        [SerializeField] private List<RarityTable> rarityTables = new();
        public List<CurrencyConfig> CurrencyData => currencyData;
        public List<RarityTable> RarityTables => rarityTables;
        public List<ModificationItemConfig> ModificationItems => modificationItems;
    }
}