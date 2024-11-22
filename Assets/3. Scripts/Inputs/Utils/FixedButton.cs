using UnityEngine;
using UnityEngine.EventSystems;

namespace _3._Scripts.Inputs.Utils
{
    public class FixedButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        public bool ButtonUp { get; private set; }
        public bool ButtonDown { get; private set; }
        public bool ButtonHold { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            ButtonDown = true;
            ButtonHold = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ButtonUp = true;
            ButtonHold = false;
        }

        private void LateUpdate()
        {
            if (ButtonDown)
                ButtonDown = false;
            
            if (ButtonUp)
                ButtonUp = false;
        }
    }
}