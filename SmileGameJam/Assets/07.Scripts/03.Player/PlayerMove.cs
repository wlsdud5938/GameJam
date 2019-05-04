using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerMove : JoystickBase
{
    public Transform target;

    public float speed = 5;
    public float rotSpeed = 3;

    public float targetRot = 0;

    public Animator animator;
    private Rigidbody rb;

    private void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        animator = transform.GetComponent<Animator>();
        joystick = GameObject.Find("MoveJoyStick").GetComponent<RectTransform>();
    }

    protected override void Start()
    {
        base.Start();
    }

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
        animator.SetFloat("RunBlend", dist);

        rb.velocity = Quaternion.Euler(0, targetRot, 0) * Vector3.forward * speed * dist;
    }

    protected override void GetJoystickUp(bool isClicked)
    {
        animator.SetFloat("RunBlend", 0);
        rb.velocity = Vector3.zero;
    }
}
