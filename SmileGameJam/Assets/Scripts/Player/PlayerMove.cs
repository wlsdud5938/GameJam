using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : JoystickBase
{
    public Transform target;
    public float speed = 5;
    public float rotSpeed = 3;

    protected override void GetJoystickDown()
    {

    }

    protected override void GetJoystickStay(float dist)
    {
        Quaternion targetRot = Quaternion.Euler(0, rotation, 0);
        target.rotation = Quaternion.Slerp(target.rotation, targetRot, Time.deltaTime * rotSpeed);
        target.position += targetRot * Vector3.forward * speed * Time.deltaTime;
    }

    protected override void GetJoystickUp(bool isClicked)
    {

    }
}
