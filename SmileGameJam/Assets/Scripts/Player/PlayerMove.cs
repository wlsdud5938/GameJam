using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : JoystickBase
{
    public Transform target;
    private PlayerAttack playerAttack;

    public float speed = 5;
    public float rotSpeed = 3;

    public float targetRot = 0;

    private bool isDead = false;

    public Animator animator;

    protected override void Update()
    {
        base.Update();
        target.rotation = Quaternion.Slerp(target.rotation, Quaternion.Euler(0, targetRot, 0), Time.deltaTime * rotSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
            Death();
    }

    protected override void GetJoystickDown()
    {
    }

    protected override void GetJoystickStay(float dist)
    {
        if (isDead) return;

        targetRot = rotation;
        if(dist > 0.2f)
            animator.SetBool("IsRunning", true);

        target.position += Quaternion.Euler(0, targetRot, 0) * Vector3.forward * speed * dist * Time.deltaTime;
        animator.SetFloat("Speed", dist);
    }

    protected override void GetJoystickUp(bool isClicked)
    {
        if (isDead) return;
        animator.SetBool("IsRunning", false);
    }

    public void Death()
    {
        animator.SetBool("IsDead", true);
        playerAttack.isDead = isDead = true;
    }
}
