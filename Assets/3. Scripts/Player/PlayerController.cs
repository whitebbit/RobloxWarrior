using _3._Scripts.Inputs;
using _3._Scripts.Inputs.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _3._Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovement _movement;
        private PlayerCamera _camera;
        private PlayerCombat _combat;

        private IInput _input;

        private void Awake()
        {
            _combat = GetComponent<PlayerCombat>();
            _movement = GetComponent<PlayerMovement>();
            _camera = GetComponent<PlayerCamera>();
        }

        private void Start()
        {
            _input = InputHandler.Instance.Input;
        }

        private void Update()
        {
            Movement();
            Camera();
            Combat();
        }

        private void Combat()
        {
            if (_input.GetAttack() && !EventSystem.current.IsPointerOverGameObject())
                _combat.Attack();

            if (_input.GetFirstAbility())
                _combat.UseFirstAbility();

            if (_input.GetSecondAbility())
                _combat.UseSecondAbility();

            if (_input.GetThirdAbility())
                _combat.UseThirdAbility();
        }

        private void Camera()
        {
            _input.CursorState();
            _camera.Look(!_input.CanLook() ? Vector2.zero : _input.GetLookAxis(), _input.SensitivityX(),
                _input.SensitivityY());
        }

        private void Movement()
        {
            _movement.Move(_input.GetMovementAxis());

            if (_input.GetJump())
                _movement.Jump();
        }
    }
}