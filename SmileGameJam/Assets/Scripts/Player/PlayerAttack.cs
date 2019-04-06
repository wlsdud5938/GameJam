using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {

    public Transform muzzle;
    private PlayerMove playerMove;

    public Animator animator;

    [Header("Skill")]
    public SkillBase nowSkill;
    public int poolSize = 15;
    public float range = 6;

    private bool isClicked = false;
    private float percent = 0, maxPercent = 100;

    [Header("Charge")]
    public int index = 0;
    public int[] powerGrade;
    public int chargeSpeed = 20;

    public Image chargeBar;

    [Header("Ultimate")]
    public UltBase nowUltimate;
    public Image ultimateBar;
    public Button ultButton;

    private int count = 0;
    public int ultCount
    {
        get
        {
            return count;
        }
        set
        {
            count = Mathf.Clamp(value, 0 , maxUltCount);
            ultimateBar.fillAmount = (float)count / maxUltCount;
            if (count >= maxUltCount)
            {
                ultimateBar.enabled = false;
                ultButton.interactable = true;
            }
        }
    }
    private int maxUltCount = 100;

    public bool isDead = false;

    public void SkillButtonDown()
    {
        if (isDead) return;
        isClicked = true;
    }

    public void SkillButtonUp()
    {
        if (isDead) return;
        if (index > 0)
        {
            animator.SetBool("IsAttacking", true);
            UseSkill();
            Invoke("FinishSkill", 0.1f);
        }

        isClicked = false;
        percent = index = 0;
        chargeBar.fillAmount = 0;
    }

    private void UseSkill()
    {
        nowSkill.UseSkill(index, range, muzzle.position, playerMove.targetRot, this);
        nowSkill.HideRange();
    }

    private void FinishSkill()
    {
        animator.SetBool("IsAttacking", false);
    }

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (isDead) return;
        if (isClicked)
        {
            percent = Mathf.Clamp(percent + Time.deltaTime * chargeSpeed, 0, maxPercent);
            chargeBar.fillAmount = percent * 0.0078f;
            if(index < 4 && powerGrade[index + 1] < percent)
                index += 1;

            if(index > 0)
            nowSkill.ShowRange(index, transform.position, playerMove.targetRot);
        }
    }

    public void ChargeUltimate(int damage)
    {
        if (isDead) return;
        ultCount += damage;
    }

    public void UseUltimate()
    {
        if (isDead) return;
        ultCount = 0;
        ultButton.interactable = false;
        ultimateBar.enabled = true;
        animator.SetBool("IsUltimating", true);
        Invoke("ShotUltimate", 0.5f);
        Invoke("FinishUltimate", 0.1f);
    }

    private void ShotUltimate()
    {
        nowUltimate.UseUltimate(muzzle.position, playerMove.targetRot);
    }

    private void FinishUltimate()
    {
        animator.SetBool("IsUltimating", false);
    }

    public void Death()
    {
        isClicked = false;
    }
}
