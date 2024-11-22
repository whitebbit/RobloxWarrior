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
            return 5;
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
            return new Vector3(touchField.AxisX, touchField.AxisY);
        }

        public bool GetAttack()
        {
            return actionButton.ButtonDown;
        }

        public bool GetFirstAbility()
        {
            return false;
        }

        public bool GetSecondAbility()
        {
            return false;
        }

        public bool GetThirdAbility()
        {
            return false;
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