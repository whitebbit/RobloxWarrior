using System;
using _3._Scripts.Config;
using _3._Scripts.Extensions;
using _3._Scripts.Localization;
using _3._Scripts.Swords.Scriptables;
using _3._Scripts.UI.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using VInspector;

namespace _3._Scripts.UI.Elements.SwordUnlocker
{
    public class SwordEgg : MonoBehaviour
    {
        [Tab("Main")] [SerializeField] private MeshRenderer egg;
        [SerializeField] private Transform swordTransform;
        [SerializeField] private ParticleSystem particle;
        [Space] [SerializeField] private TMP_Text swordRarityText;

        [Tab("Egg Animation")] [SerializeField]
        private float eggScaleDuration = 1f;

        [SerializeField] private float eggScaleFactor = 1f;
        [SerializeField] private float eggShrinkDuration = 0.5f;

        [Tab("Sword Animation")] [SerializeField]
        private float swordDuration = 1f;

        [SerializeField] private float swordScaleFactor = 1f;
        [SerializeField] private float swordWaitDuration = 1f;
        [SerializeField] private float swordShrinkDuration = 0.5f;


        private LocalizeStringEvent _swordRarityLocalizeString;
        private Transform _currentSword;
        private int _eggHealth = 3;

        private Vector3 _eggStartPosition;
        private event Action OnFinished;
        private event Action OnDestroyed;

        private void Awake()
        {
            _swordRarityLocalizeString = swordRarityText.GetComponent<LocalizeStringEvent>();
            _eggStartPosition = egg.transform.localPosition;
        }

        public void SetSword(Material eggMaterial, SwordConfig swordConfig)
        {
            var sword = Instantiate(swordConfig.Prefab, swordTransform);
            var rarity = Configuration.Instance.GetRarityTable(swordConfig.Rarity);
            var main = particle.main;

            swordRarityText.color = rarity.MainColor;
            _swordRarityLocalizeString.SetReference(rarity.TitleID);

            _currentSword = sword.transform;
            _currentSword.localScale = Vector3.zero;
            _eggHealth = 3;

            egg.material = eggMaterial;

            EggAnimation();

            main.startColor = rarity.MainColor;
            sword.gameObject.SetLayer("UI");
        }

        public void GetDamage()
        {
            egg.transform.DOShakePosition(.5f, 50, 15)
                .OnComplete(() => egg.transform.DOLocalMove(_eggStartPosition, 0.1f));
            egg.transform.DOShakeRotation(.75f, 25, 15);

            _eggHealth--;

            if (_eggHealth > 0) return;

            egg.transform.DOScale(0, eggShrinkDuration)
                .OnComplete(() => SwordAnimation(_currentSword))
                .SetDelay(1)
                .SetEase(Ease.InBack);

            OnDestroyed?.Invoke();
        }

        public void SetOnFinished(Action action) => OnFinished = action;
        public void SetOnDestroyed(Action action) => OnDestroyed = action;

        private void EggAnimation()
        {
            var eggTransform = egg.transform;

            eggTransform.localEulerAngles = Vector3.zero;
            eggTransform.localScale = Vector3.zero;
            eggTransform.DOScale(eggScaleFactor, eggScaleDuration).SetEase(Ease.OutBack).SetDelay(.5f);

            swordRarityText.Fade(0);
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
    }
}