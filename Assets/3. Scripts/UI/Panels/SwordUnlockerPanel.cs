using System;
using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Extensions;
using _3._Scripts.Localization;
using _3._Scripts.Swords.Scriptables;
using _3._Scripts.UI.Elements.SwordUnlocker;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Panels.Base;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using VInspector;
using Random = UnityEngine.Random;

namespace _3._Scripts.UI.Panels
{
    public class SwordUnlockerPanel : SimplePanel
    {
        [SerializeField] private List<SwordEgg> swordEggs = new();
        [SerializeField] private TMP_Text tutorialText;
        
        private bool _started;

        private int _eggsCount;
        private Sequence _sequenceTutorial;

        public void StartUnlocking(int eggsCount, Material eggMaterial, SwordConfig swordConfig, Action onFinished)
        {
            _eggsCount = eggsCount;

            TutorialAnimation();
        }

        private void Update()
        {
            if (!_started) return;
            
            _started = false;
            _sequenceTutorial.Kill();
        }
        
        private void TutorialAnimation()
        {
            _sequenceTutorial = DOTween.Sequence();

            var randomRotation = Random.Range(-5, 5);
            var randomScale = Random.Range(1.2f, 1.3f);

            _sequenceTutorial
                .Append(tutorialText.transform.DOScale(randomScale, 0.5f))
                .Join(tutorialText.transform.DORotate(new Vector3(0, 0, randomRotation), 0.5f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad).OnKill(() => tutorialText.DOFade(0, 0.1f))
                .OnStart(() => tutorialText.DOFade(1, 0.1f));
        }
    }
}