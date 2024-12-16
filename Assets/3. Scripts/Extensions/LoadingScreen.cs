using System.Collections;
using _3._Scripts.Singleton;
using _3._Scripts.UI.Transitions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Extensions
{
    public class LoadingScreen : Singleton<LoadingScreen>
    {
        [SerializeField] private FadeTransition transition;

        [SerializeField] private Slider progressBar;

        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            transition.ForceOut();

            if (progressBar != null)
                progressBar.value = 0;
        }

        public void ShowLoadingScreen(IEnumerator coroutine)
        {
            StartCoroutine(ExecuteWithLoadingScreen(coroutine));
        }

        private Tween _progressTween;

        private IEnumerator ExecuteWithLoadingScreen(IEnumerator coroutine)
        {
            transition.AnimateIn();

            progressBar.value = 0;
            _progressTween = progressBar.DOValue(1, 3);

            yield return StartCoroutine(coroutine);

            _progressTween.Kill();
            progressBar.value = 1.0f;

            transition.AnimateOut();
        }
    }
}