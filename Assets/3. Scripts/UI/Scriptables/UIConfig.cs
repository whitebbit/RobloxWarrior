using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.UI.Scriptables
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Configs/UI/UI Config",
        order = 0)]    
    public class UIConfig : ScriptableObject
    {
        [SerializeField] private List<ModificationItemConfig> modificationItems = new();

        public List<ModificationItemConfig> ModificationItems => modificationItems;
    }
}