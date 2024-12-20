﻿using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Singleton;
using UnityEngine;
using YG;

namespace _3._Scripts.Tutorial
{
    public class TutorialManager : Singleton<TutorialManager>
    {
        [SerializeField] private List<TutorialStep> steps = new();
        [SerializeField] private TutorialArrow arrow;

        private Player.Player Player => _Scripts.Player.Player.Instance;
        private TutorialStep _currentStep;


        private void Start()
        {
            StartStep("start");
        }

        public void StartStep(string id)
        {
            if (YG2.saves.Tutorials.ContainsKey(id) && YG2.saves.Tutorials[id]) return;

            var step = steps.FirstOrDefault(s => s.ID == id);
            if (step == null) return;

            if (step.Target)
                arrow.Enable(Player.transform, step.Target);

            if (step.TutorialObjects.Count > 0)
            {
                foreach (var item in step.TutorialObjects)
                {
                    item.Object.gameObject.SetActive(true);
                }
            }

            _currentStep = step;
        }

        public void DisableStep(string id)
        {
            if (YG2.saves.Tutorials.ContainsKey(id) && YG2.saves.Tutorials[id]) return;

            var step = steps.FirstOrDefault(s => s.ID == id);
            if (step == null || _currentStep != step) return;

            YG2.saves.Tutorials.Add(id, true);

            if (step.TutorialObjects.Count > 0)
            {
                foreach (var item in step.TutorialObjects.Where(item => item.DisableOnComplete))
                {
                    item.Object.gameObject.SetActive(false);
                }
            }

            arrow.Disable();
        }
    }
}