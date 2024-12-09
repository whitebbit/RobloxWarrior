using System;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.UI.Elements.SwordsPanel;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public abstract class CollectionItem<T, TSelf> : MonoBehaviour, IInitializable<T>
        where TSelf : CollectionItem<T, TSelf>
    {
        [SerializeField] protected RawImage icon;
        [SerializeField] protected Image focusImage;

        private Button _button;

        public event Action<TSelf> OnSelect;
        public T Config { get; private set; }
        protected abstract TSelf Self { get; }
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
        }
        
        private void OnClick()
        {
            OnSelect?.Invoke(Self);

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
            Config = config;
        }
    }
}