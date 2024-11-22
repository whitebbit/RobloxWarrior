using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.UI.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIScreen : MonoBehaviour
    {
        public string ID => id;
        public bool Blocked { get; private set; }
        public bool Opened { get; private set; }

        [SerializeField] private string id;
        //[SerializeField] private List<UIPanel> defaultPanels = new();

        private UIPanel[] panels;
        private bool onTransition;
        private CanvasGroup canvasGroup;

        public void Initialize()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            panels = GetComponentsInChildren<UIPanel>();
            foreach (var panel in panels)
            {
                panel.Initialize();
                panel.ForceClose();
            }
            Opened = false;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }

        public void Open(float duration = 0f, TweenCallback onStart = null, TweenCallback onComplete = null)
        {
            if (onTransition) return;
            if (Opened) return;

            gameObject.SetActive(true);
            onTransition = true;

            canvasGroup.DOFade(1, duration).OnStart(() =>
            {
                transform.SetAsLastSibling();
                //foreach (var panel in defaultPanels) panel.Enabled = true;
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                Opened = true;
                canvasGroup.blocksRaycasts = true;
                onTransition = false;
                onComplete?.Invoke();
            });
        }

        public void Close(float duration = 0f, TweenCallback onStart = null, TweenCallback onComplete = null)
        {
            if (onTransition) return;
            if (!Opened) return;
            onTransition = true;
            StartCoroutine(WaitClose(duration, onStart, onComplete));
        }

        public T GetPanel<T>() where T : UIPanel
        {
            return (T)panels.FirstOrDefault(p => p is T);
        }
        
        
        private IEnumerator WaitClose(float duration = 0f, TweenCallback onStart = null,
            TweenCallback onComplete = null)
        {
            foreach (var panel in panels) panel.Enabled = false;
            yield return new WaitUntil(() => panels.All(p => !p.Enabled));
            canvasGroup.DOFade(0, duration).OnStart(() =>
            {
                canvasGroup.blocksRaycasts = false;
                Opened = false;
                onStart?.Invoke();
            }).OnComplete(() =>
            {
                transform.SetAsFirstSibling();
                onTransition = false;
                onComplete?.Invoke();
                gameObject.SetActive(false);
            });
        }
    }
}