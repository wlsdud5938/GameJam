using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public float rotSpeed = 3;
    public float targetRot = 0;

    [Header("Roll")]
    public float rollPower = 30;
    public float rollDelay = 0.5f;
    private bool isRolling = false;
    private float rollTime = 0.35f, rollnowDelay;

    public void MoveJoystickStay(float dist, float rotation)
    {
        targetRot = rotation;
        animator.SetFloat("RunBlend", dist);

        if (!isRolling)
        {
            rb.velocity = Quaternion.Euler(0, targetRot, 0) * Vector3.forward * moveSpeed * dist;
            animator.speed = moveSpeed * 0.3f;
        }
    }

    public void MoveJoystickUp(bool isMoved)
    {
        animator.SetFloat("RunBlend", 0);
        if (!isRolling)
            rb.velocity = Vector3.zero;
    }

    public void Roll()
    {
        if(rollnowDelay <= 0)
        StartCoroutine(RollAnim());   
    }

    IEnumerator RollAnim()
    {
        isRolling = true;
        rb.AddForce(Quaternion.Euler(0, targetRot, 0) * Vector3.forward * rollPower, ForceMode.Impulse);
        yield return new WaitForSeconds(rollTime);
        isRolling = false;
        rb.velocity = Vector3.zero;
        rollnowDelay = rollDelay;
    }
}