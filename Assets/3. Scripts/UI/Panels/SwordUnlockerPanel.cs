using System;
using System.Collections;
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
using UnityEngine.UI;
using VInspector;
using Random = UnityEngine.Random;

namespace _3._Scripts.UI.Panels
{
    public class SwordUnlockerPanel : SimplePanel
    {
        [SerializeField] private List<SwordEgg> swordEggs = new();
        [SerializeField] private TMP_Text tutorialText;
        [SerializeField] private Button disableAutoOpen;

        private bool _started;

        private int _eggsCount;
        private Sequence _sequenceTutorial;
        private bool _autoOpen;
        protected override void OnOpen()
        {
            base.OnOpen();
            foreach (var swordEgg in swordEggs)
            {
                swordEgg.gameObject.SetActive(false);
            }
        }

        public void StartUnlocking(Material eggMaterial, List<SwordConfig> swordConfigs, Action onFinished)
        {
            _eggsCount = swordConfigs.Count;
            swordEggs[0].SetOnFinished(onFinished);
            swordEggs[0].SetOnDestroyed(() =>
            {
                _started = false;
                _sequenceTutorial.Kill();
                StopAllCoroutines();
            });

            for (var i = 0; i < _eggsCount; i++)
            {
                swordEggs[i].SetSword(eggMaterial, swordConfigs[i]);
                swordEggs[i].gameObject.SetActive(true);
            }

            TutorialAnimation();

            DisableAutoOpen();

            _started = true;
        }

        public void EnableAutoOpen(Action onDisable)
        {
            _autoOpen = true;
            
            disableAutoOpen.gameObject.SetActive(true);
            
            disableAutoOpen.onClick.RemoveAllListeners();
            disableAutoOpen.onClick.AddListener(() =>
            {
                onDisable?.Invoke();
                DisableAutoOpen();
            });
            
            StartCoroutine(AutoOpen());
        }

        private void DisableAutoOpen()
        {
            _autoOpen = false;
            disableAutoOpen.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            if (!_started) return;

            if (!Input.GetMouseButtonDown(0)) return;

            DamageEgg();
        }

        private void DamageEgg()
        {
            for (var i = 0; i < _eggsCount; i++)
            {
                swordEggs[i].GetDamage();
            }
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

        private IEnumerator AutoOpen()
        {
            if(!_autoOpen) yield break;

            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                DamageEgg();
            }
        }
    }
}