using UnityEngine;

namespace _3._Scripts.Inputs.Interfaces
{
    public interface IInput
    {
        public float SensitivityX();
        public float SensitivityY();

        public Vector2 GetMovementAxis();
        public Vector2 GetLookAxis();

        public bool GetAttack();
        
        public bool GetFirstAbility();
        public bool GetSecondAbility();
        public bool GetThirdAbility();
        
        public bool GetJump();

        public bool CanLook();

        public void CursorState();
    }
}
