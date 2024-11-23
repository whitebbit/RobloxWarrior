using System;
using _3._Scripts.Config;
using _3._Scripts.Extensions;
using _3._Scripts.Localization;
using _3._Scripts.Swords.Scriptables;
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
        [Tab("Main")]
        [SerializeField] private MeshRenderer egg;
        [Space] 
        [SerializeField] private Transform swordTransform;
        [Space] 
        [SerializeField] private TMP_Text swordRarityText;
        [SerializeField] private LocalizeStringEvent swordRarityLocalizeString;
        [Space] 
        [SerializeField] private TMP_Text tutorialText;
        [Space] [SerializeField] private ParticleSystem particle;
        
        [Tab("Egg Animation")] [SerializeField]
        private float eggScaleDuration = 1f;

        [SerializeField] private float eggScaleFactor = 1f;
        [SerializeField] private float eggShrinkDuration = 0.5f;

        [Tab("Sword Animation")] [SerializeField]
        private float swordDuration = 1f;

        [SerializeField] private float swordScaleFactor = 1f;
        [SerializeField] private float swordWaitDuration = 1f;
        [SerializeField] private float swordShrinkDuration = 0.5f;

        private event Action OnFinished;

        private Transform _currentSword;
        private Vector3 _eggStartPosition;

        private bool _started;
        private int _eggHealth = 3;

        public override void Initialize()
        {
            base.Initialize();
            _eggStartPosition = egg.transform.localPosition;
        }

        public void StartUnlocking(Material eggMaterial, SwordConfig swordConfig, Action onFinished)
        {
            var sword = Instantiate(swordConfig.Prefab, swordTransform);
            var rarity = Configuration.Instance.GetRarityTable(swordConfig.Rarity);
            var main = particle.main;

            OnFinished = onFinished;

            swordRarityText.color = rarity.MainColor;
            swordRarityLocalizeString.SetReference(rarity.TitleID);

            _currentSword = sword.transform;
            _currentSword.localScale = Vector3.zero;

            egg.material = eggMaterial;

            
            TutorialAnimation();
            EggAnimation();

            main.startColor = rarity.MainColor;
            sword.gameObject.SetLayer("UI");
        }

        private void Update()
        {
            if (!_started) return;

            if (_eggHealth <= 0)
            {
                _started = false;
                _sequence.Kill();
                egg.transform.DOScale(0, eggShrinkDuration)
                    .OnComplete(() => SwordAnimation(_currentSword))
                    .SetDelay(1)
                    .SetEase(Ease.InBack);
            }
            else
            {
                if (!Input.GetMouseButtonDown(0)) return;
                egg.transform.DOShakePosition(.5f, 50, 15)
                    .OnComplete(() => egg.transform.DOLocalMove(_eggStartPosition, 0.1f));
                egg.transform.DOShakeRotation(.75f, 25, 15);
                _eggHealth--;
            }
        }

        private void EggAnimation()
        {
            var eggTransform = egg.transform;

            eggTransform.localScale = Vector3.zero;
            eggTransform.DOScale(eggScaleFactor, eggScaleDuration).SetEase(Ease.OutBack).SetDelay(.5f);

            swordRarityText.Fade(0);

            _eggHealth = 3;
            _started = true;
        }

        private void SwordAnimation(Transform sword)
        {
            sword.localScale = Vector3.zero;
            sword.localEulerAngles = Vector3.zero;

            swordRarityText.DOFade(1, 0.1f);

            var sequence = DOTween.Sequence();

            // Увеличиваем объект и одновременно вращаем его
            sequence.Append(sword.DOScale(swordScaleFactor, swordDuration).SetEase(Ease.OutBack)) // Увеличиваем
                .Join(sword.DORotate(new Vector3(45, 270, 0), swordDuration,
                    RotateMode.FastBeyond360)) // Вращаем
                .AppendInterval(swordWaitDuration)
                .Append(sword.DOScale(Vector3.zero, swordShrinkDuration).SetEase(Ease.InBack).OnStart(() =>
                {
                    swordRarityText.DOFade(0, 0.1f);
                }))
                .OnComplete(() =>
                {
                    particle.Stop();
                    Destroy(sword.gameObject);
                    OnFinished?.Invoke();
                }); // Уменьшаем до 0

            // Запускаем анимацию
            sequence.Play();
            
            particle.Play();
        }

        private Sequence _sequence;

        private void TutorialAnimation()
        {
            // Создаём последовательность анимаций
            _sequence = DOTween.Sequence();

            var randomRotation = Random.Range(-5, 5);
            var randomScale = Random.Range(1.2f, 1.3f);

            _sequence
                .Append(tutorialText.transform.DOScale(randomScale, 0.5f))
                .Join(tutorialText.transform.DORotate(new Vector3(0, 0, randomRotation), 0.5f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad).OnKill(() => tutorialText.DOFade(0, 0.1f))
                .OnStart(() => tutorialText.DOFade(1, 0.1f));
        }
    }
}