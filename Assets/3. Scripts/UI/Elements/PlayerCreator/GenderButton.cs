using System;
using _3._Scripts.Player.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements.PlayerCreator
{
    public class GenderButton: MonoBehaviour
    {
        [SerializeField] private PlayerGender type;
        [Tab("Components")]
        [SerializeField] private Image focusImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text text;
        [Tab("Colors")]
        [SerializeField] private Color selectedColor; 
        [SerializeField] private Color unselectedColor;

        public event Action<PlayerGender, GenderButton> OnClick; 
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
            text.color = selected? selectedColor : unselectedColor;
            iconImage.color = selected? selectedColor : unselectedColor;
        }

        private void OnClickButton()
        {
            OnClick?.Invoke(type,this);
        }
    }
}