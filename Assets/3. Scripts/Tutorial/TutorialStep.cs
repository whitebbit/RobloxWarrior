using System;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    [Serializable]
    public class TutorialStep
    {
        [SerializeField] private string id;
        [SerializeField] private Transform target;

        public string ID => id;
        public Transform Target => target;
    }
}