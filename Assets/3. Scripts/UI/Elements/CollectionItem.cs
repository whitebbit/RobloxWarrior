using System;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.UI.Elements.SwordsPanel;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public abstract class CollectionItem<T, TSelf, TIcon> : MonoBehaviour, IInitializable<T>
        where TSelf : CollectionItem<T, TSelf, TIcon> where TIcon : MonoBehaviour
    {
        [Tab("Collection Item")]
        [SerializeField] protected TIcon icon;
        [SerializeField] protected Image focusImage;

        private Button _button;

        public event Action<TSelf> OnSelect;
        public T Config { get; protected set; }
        protected abstract TSelf Self { get; }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
            OnStart();
        }

        protected virtual void OnStart()
        {
            
        }
        private void OnClick()
        {
            OnSelect?.Invoke(Self);
        }

        protected void ResetOnSelect()
        {
            OnSelect = null;
        }

        /// <summary>
        /// Отключить фокус.
        /// </summary>
        public void DisableFocus() => focusImage.gameObject.SetActive(false);

        /// <summary>
        /// Установить фокус как текущий (желтый цвет).
        /// </summary>
        public void SetCurrentFocus()
        {
            focusImage.gameObject.SetActive(true);
            focusImage.color = Color.yellow;
        }

        /// <summary>
        /// Установить фокус как выбранный (зеленый цвет).
        /// </summary>
        public void SetSelectedFocus()
        {
            focusImage.gameObject.SetActive(true);
            focusImage.color = Color.green;
        }

        public virtual void Initialize(T config)
        {
            if (config == null)
            {
                icon.gameObject.SetActive(false);
                return;
            }

            icon.gameObject.SetActive(true);
            Config = config;
        }
    }
}