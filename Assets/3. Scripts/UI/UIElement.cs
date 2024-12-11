using System;
using _3._Scripts.UI.Interfaces;
using DG.Tweening;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI
{
    public abstract class UIElement : MonoBehaviour, IUIElement
    {
        private bool _enabled;
        private bool onTransition;
        public abstract IUITransition InTransition { get; set; }
        public abstract IUITransition OutTransition { get; set; }

        [Button]
        private void SwitchState()
        {
            Enabled = !Enabled;
        }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value)
                {
                    Open();
                }
                else
                {
                    Close();
                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private Tween currentTween;

        public abstract void Initialize();

        public virtual void ForceOpen()
        {
            gameObject.SetActive(true);
            InTransition.ForceIn();
            _enabled = true;
            OnOpen();
        }

        public virtual void ForceClose()
        {
            OnClose();
            OutTransition.ForceOut();
            _enabled = false;
            gameObject.SetActive(false);
        }

        public event Action<UIElement> OnOpenEvent;

        private void Open()
        {
            if (onTransition)
            {
                currentTween?.Pause();
                currentTween?.Kill();
                currentTween = null;
            }

            onTransition = true;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            OnOpenEvent?.Invoke(this);
            currentTween = InTransition.AnimateIn().OnComplete(() =>
            {
                _enabled = true;
                onTransition = false;
            });
            OnOpen();
        }

        public event Action<UIElement> OnCloseEvent;

        private void Close()
        {
            if (onTransition)
            {
                currentTween?.Pause();
                currentTween?.Kill();
                currentTween = null;
            }

            onTransition = true;
            OnCloseEvent?.Invoke(this);
            currentTween = OutTransition.AnimateOut().OnComplete(() =>
            {
                OnClose();
                _enabled = false;
                onTransition = false;
                transform.SetAsFirstSibling();
                gameObject.SetActive(false);
            });
        }

        protected virtual void OnOpen()
        {
        }

        protected virtual void OnClose()
        {
        }
    }
}