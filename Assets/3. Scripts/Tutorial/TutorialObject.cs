using System;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    [Serializable]
    public class TutorialObject
    {
        [SerializeField] private Transform tutorialObject;
        [SerializeField] private bool disableOnComplete;
        
        public Transform Object => tutorialObject;
        public bool DisableOnComplete => disableOnComplete;
    }
}