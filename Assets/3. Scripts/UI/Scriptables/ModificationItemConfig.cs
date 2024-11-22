using _3._Scripts.UI.Enums;
using UnityEngine;

namespace _3._Scripts.UI.Scriptables
{
    [CreateAssetMenu(fileName = "ModificationItemConfig", menuName = "Configs/UI/Modification Panel/Modification Item",
        order = 0)]
    public class ModificationItemConfig : ScriptableObject
    {
        [SerializeField] private ModificationType type;
        [Space] 
        
        [SerializeField] private Sprite icon;
        [SerializeField] private string titleID;
        [SerializeField] private string descriptionID;
        [SerializeField] private Color color;
        
        public ModificationType Type => type;

        public Sprite Icon => icon;
        public string TitleID => titleID;
        public string DescriptionID => descriptionID;
        public Color Color => color;
    }
}