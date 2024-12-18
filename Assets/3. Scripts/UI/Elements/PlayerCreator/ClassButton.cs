using System;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Localization;
using _3._Scripts.Player.Enums;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements.PlayerCreator
{
    public class ClassButton : MonoBehaviour, IInitializable<PlayerClass>
    {
        [Tab("Components")] [SerializeField] private Image focusImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text text;
        [SerializeField] private LocalizeStringEvent textLocalize;
        [Tab("Colors")] [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;

        public event Action<PlayerClass, ClassButton> OnClick;
        private PlayerClass _config;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(OnClickButton);
        }

        public void SelectedState(bool selected)
        {
            focusImage.gameObject.SetActive(selected);
            text.color = selected ? selectedColor : unselectedColor;
            iconImage.color = selected ? selectedColor : unselectedColor;
        }

        private void OnClickButton()
        {
            OnClick?.Invoke(_config, this);
        }

        public void Initialize(PlayerClass config)
        {
            _config = config;
            iconImage.sprite = _config.Icon;
            iconImage.ScaleImage();
            textLocalize.SetReference(_config.TitleID);
        }
    }
}