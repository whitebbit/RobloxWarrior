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
        private bool _onTransition;
        public abstract IUITransition InTransition { get; set; }
        public abstract IUITransition OutTransition { get; set; }


        public void SwitchState()
        {
            Enabled = !Enabled;
        }


        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_onTransition || value == _enabled) return;

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

        private Tween _currentTween;

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
            if (_onTransition)
            {
                _currentTween?.Pause();
                _currentTween?.Kill();
                _currentTween = null;
            }

            _onTransition = true;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            OnOpenEvent?.Invoke(this);
            _currentTween = InTransition.AnimateIn().OnComplete(() =>
            {
                _enabled = true;
                _onTransition = false;
            });
            OnOpen();
        }

        public event Action<UIElement> OnCloseEvent;

        private void Close()
        {
            if (_onTransition)
            {
                _currentTween?.Pause();
                _currentTween?.Kill();
                _currentTween = null;
            }

            _onTransition = true;
            OnCloseEvent?.Invoke(this);
            _currentTween = OutTransition.AnimateOut().OnComplete(() =>
            {
                OnClose();
                _enabled = false;
                _onTransition = false;
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