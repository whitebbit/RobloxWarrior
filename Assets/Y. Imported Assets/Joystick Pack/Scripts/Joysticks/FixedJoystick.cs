using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    [SerializeField] private Transform leftUp;
    [SerializeField] private Transform leftDown;
    [SerializeField] private Transform rightUp;
    [SerializeField] private Transform rightDown;
    private void Update()
    {
        if (Direction == Vector2.zero)
        {
            rightUp.gameObject.SetActive(false);
            rightDown.gameObject.SetActive(false);
            leftUp.gameObject.SetActive(false);
            leftDown.gameObject.SetActive(false);
            return;
        }

        var isRight = Direction.x > 0;
        var isLeft = Direction.x < 0;
        var isUp = Direction.y > 0;
        var isDown = Direction.y < 0;

        rightUp.gameObject.SetActive(isRight && isUp);
        rightDown.gameObject.SetActive(isRight && isDown);
        leftUp.gameObject.SetActive(isLeft && isUp);
        leftDown.gameObject.SetActive(isLeft && isDown);
    }
}