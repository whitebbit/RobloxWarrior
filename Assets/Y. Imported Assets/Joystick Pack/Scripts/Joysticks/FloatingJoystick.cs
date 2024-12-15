using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    [SerializeField] private Transform leftUp;
    [SerializeField] private Transform leftDown;
    [SerializeField] private Transform rightUp;
    [SerializeField] private Transform rightDown;

    private Vector3 _startPosition;

    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(true);
    }

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

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(true);
        base.OnPointerUp(eventData);
    }
}