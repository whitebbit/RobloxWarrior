using System;
using _3._Scripts.Saves.Interfaces;
using _3._Scripts.UI.Interfaces;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Config.Scriptables
{
    public abstract class ConfigObject<T> : ScriptableObject, ISavable, IUIObject<T>
    {
        [Tab("Config")]
        [Header("Save")]
        [SerializeField] private string id;
        [Header("UI")]
        [SerializeField] private string titleID;
        [SerializeField] private string descriptionID;
        
        public string ID => id;
        public string TitleID => titleID;
        public string DescriptionID => descriptionID;
        
        public abstract T Icon { get; }
    }
}