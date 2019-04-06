using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : JoystickBase
{
    public Transform target;
    public float speed = 5;
    public float rotSpeed = 3;

    public float targetRot = 0;

    protected override void Update()
    {
        base.Update();
        target.rotation = Quaternion.Slerp(target.rotation, Quaternion.Euler(0, targetRot, 0), Time.deltaTime * rotSpeed);
    }

    protected override void GetJoystickDown()
    {

    }

    protected override void GetJoystickStay(float dist)
    {
        targetRot = rotation;
        target.position += Quaternion.Euler(0, targetRot, 0) * Vector3.forward * speed * Time.deltaTime;
    }

    protected override void GetJoystickUp(bool isClicked)
    {

    }
}
