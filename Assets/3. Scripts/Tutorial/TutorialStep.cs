using System;
using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    [Serializable]
    public class TutorialStep
    {
        [SerializeField] private string id;
        [SerializeField] private List<TutorialObject> tutorialObjects = new();
        
        [SerializeField] private Transform target;

        public string ID => id;
        public Transform Target => target;

        public List<TutorialObject> TutorialObjects => tutorialObjects;
    }
}