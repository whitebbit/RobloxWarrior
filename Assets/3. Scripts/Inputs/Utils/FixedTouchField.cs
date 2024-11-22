using System;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _3._Scripts.Inputs.Utils
{
    public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool Pressed { get; private set; }
        public float AxisX { get; private set; }
        public float AxisY { get; private set; }

        private Vector2 _previousPosition;
        private int _touchId = -1; // Store the ID of the touch controlling the field
        

        private void Update()
        {
            if (!Pressed) return;

            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    // Ensure we are only tracking the touch that started the interaction with this panel
                    if (touch.fingerId == _touchId)
                    {
                        var currentTouchPosition = touch.position;
                        var delta = currentTouchPosition - _previousPosition;

                        AxisX = delta.x / Screen.width / Time.deltaTime;
                        AxisY = delta.y / Screen.height / Time.deltaTime;

                        _previousPosition = currentTouchPosition;
                    }
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Pressed = true;
            _previousPosition = eventData.position;
            AxisX = 0;
            AxisY = 0;

            // Find the touch ID corresponding to this pointer event
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.position == eventData.position)
                    {
                        _touchId = touch.fingerId;
                        break;
                    }
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Pressed = false;
            AxisX = 0;
            AxisY = 0;
            _touchId = -1; // Reset the touch ID when the interaction ends
        }
    }
}