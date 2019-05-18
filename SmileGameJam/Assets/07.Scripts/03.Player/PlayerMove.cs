﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public float rotSpeed = 3;
    public float targetRot = 0;
    private bool isMoving = false;

    [Header("Roll")]
    public float rollPower = 30;
    public float rollJumpPower = 1.1f;
    public float rollDelay = 0.5f, rollCancelDelay = 0.1f, rollJumpPercent = 0.6f;

    private bool isRolling = false;
    private float rollTime = 0.35f, rollnowDelay;

    public void MoveJoystickStay(float dist, float rotation)
    {
        isMoving = true;
        animator.SetFloat("RunBlend", dist);

        if (!isRolling)
        {
            targetRot = rotation;
            rb.velocity = Quaternion.Euler(0, targetRot, 0) * Vector3.forward * moveSpeed * dist;
            animator.speed = moveSpeed * 0.3f;
        }
    }

    public void MoveJoystickUp(bool isMoved)
    {
        isMoving = false;
        animator.SetFloat("RunBlend", 0);
        if (!isRolling)
            rb.velocity = Vector3.zero;
    }

    public void Roll()
    {
        if (!isRolling)
            isRolling = true;
    }

    public void RollAnim()
    {
        rollnowDelay += Time.deltaTime;

        if (rollnowDelay > rollDelay + rollCancelDelay) //구르기 끝
        {
            isRolling = false;
            rollnowDelay = 0;
        }
        else if (rollnowDelay > rollDelay) //구르기 후딜레이 시작
        {
            rb.velocity = Vector3.zero;
        }
        else if (rollnowDelay < rollDelay * rollJumpPercent)
        {
            rb.velocity = Quaternion.Euler(0, targetRot, 0) * Vector3.forward * rollPower * rollJumpPower;
            Debug.Log(rollPower * rollJumpPower);
        }
        else if (rollnowDelay >= rollDelay * rollJumpPercent && rollnowDelay < rollDelay)
        {
            rb.velocity = Quaternion.Euler(0, targetRot, 0) * Vector3.forward * rollPower;
            Debug.Log(rollPower);
        }
    }
}