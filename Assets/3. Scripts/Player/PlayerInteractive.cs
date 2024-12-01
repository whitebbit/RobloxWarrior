using System;
using _3._Scripts.Detectors.OverlapSystem;
using _3._Scripts.Extensions.Interfaces;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class PlayerInteractive : MonoBehaviour
    {
        [SerializeField] private InteractiveDetector detector;

        private IInteractive _interactive;

        private void Start()
        {
            detector.DetectState(true);
            detector.OnFound += OnFound;
        }

        private void OnFound(IInteractive obj)
        {
            if (obj == null)
            {
                _interactive?.StopInteracting();
                _interactive = null;
            }
            else
            {
                if (_interactive == obj) return;

                _interactive?.StopInteracting();

                _interactive = obj;
                _interactive.Interact();
            }
        }
    }
}