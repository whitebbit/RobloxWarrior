using System;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Inputs.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Inputs
{
    public class MobileInput : MonoBehaviour, IInput
    {
        [Tab("Input Components")] 
        [SerializeField]
        private Joystick joystick;

        [SerializeField] private FixedTouchField touchField;
        [SerializeField] private FixedButton jumpButton;
        [SerializeField] private FixedButton actionButton;
        [Tab("Abilities")] 
        [SerializeField] private FixedButton firstAbilityButton;
        [SerializeField] private FixedButton secondAbilityButton;
        [SerializeField] private FixedButton thirdAbilityButton;

        private CanvasGroup _canvas;

        private void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();
        }

        public void SetState(bool state)
        {
            _canvas.alpha = state ? 1 : 0;
        }

        public float SensitivityX()
        {
            return 500;
        }

        public float SensitivityY()
        {
            return 2;
        }

        public Vector2 GetMovementAxis()
        {
            return joystick.Direction;
        }
        
        public Vector2 GetLookAxis()
        {
            var axis = new Vector3(touchField.AxisX, touchField.AxisY);
            return axis;
        }
        
        public bool GetAttack()
        {
            return actionButton.ButtonDown;
        }

        public bool GetFirstAbility()
        {
            return firstAbilityButton.ButtonDown;
        }

        public bool GetSecondAbility()
        {
            return secondAbilityButton.ButtonDown;
        }

        public bool GetThirdAbility()
        {
            return thirdAbilityButton.ButtonDown;
        }

        public bool GetJump()
        {
            return jumpButton.ButtonDown;
        }


        public bool CanLook()
        {
            return touchField.Pressed;
        }

        public void CursorState()
        {
            
        }
    }
}