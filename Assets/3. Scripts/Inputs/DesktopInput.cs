using _3._Scripts.Inputs.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _3._Scripts.Inputs
{
    public class DesktopInput : IInput
    {
        public float SensitivityX()
        {
            return 500f;
        }

        public float SensitivityY()
        {
            return 2.5f;
        }

        public Vector2 GetMovementAxis()
        {
            return new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        
        public Vector2 GetLookAxis()
        {
            return new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }
        
        public bool GetAttack()
        {
            return Input.GetMouseButtonDown(0);
        }

        public bool GetFirstAbility()
        {
            return Input.GetKeyDown(KeyCode.Q);
        }

        public bool GetSecondAbility()
        {
            return Input.GetKeyDown(KeyCode.E);
        }

        public bool GetThirdAbility()
        {
            return Input.GetKeyDown(KeyCode.R);
        }

        public bool GetJump()
        {
            if (!Input.GetKeyDown(KeyCode.Space)) return false;
            
            EventSystem.current.SetSelectedGameObject(null);
            return true;
        }
        
        public bool GetInteract()
        {
            return Input.GetKeyDown(KeyCode.E);
        }

        
        public bool CanLook()
        {
            return Input.GetMouseButton(1);
        }

        public void CursorState()
        {
            Cursor.visible = !CanLook();
            Cursor.lockState = CanLook() ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}