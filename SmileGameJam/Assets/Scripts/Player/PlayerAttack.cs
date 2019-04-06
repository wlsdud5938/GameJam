using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {

    public Transform muzzle;
    private PlayerMove playerMove;

    [Header("Skill")]
    public SkillBase nowSkill;
    public int poolSize = 15;
    public float range = 6;

    private bool isClicked = false;
    private float percent = 0, maxPercent = 100;

    [Header("Charge")]
    public int index = 0;
    //[HideInInspector]
    public int[] powerGrade;
    public int chargeSpeed = 20;

    public Image chargeBar;

    [Header("Ultimate")]
    public UltBase nowUltimate;

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
        }
    }
    public int maxUltCount = 3;

    public void SkillButtonDown()
    {
        isClicked = true;
    }

    public void SkillButtonUp()
    {
        nowSkill.UseSkill(index, range, 
            transform.position/* + Quaternion.Euler(0,playerMove.targetRot,0) * Vector3.forward * 0.2f*/, playerMove.targetRot, ChargeUltimate);
        nowSkill.HideRange();

        isClicked = false;
        percent = index = 0;
        chargeBar.fillAmount = 0;
    }

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (isClicked)
        {
            percent = Mathf.Clamp(percent + Time.deltaTime * chargeSpeed, 0, maxPercent);
            chargeBar.fillAmount = percent * 0.01f;
            if(index < 4 && powerGrade[index + 1] < percent)
                index += 1;

            nowSkill.ShowRange(index, transform.position + Quaternion.Euler(0, playerMove.targetRot, 0) * Vector3.forward * 0.2f, playerMove.targetRot);
        }
    }

    public void ChargeUltimate()
    {
        ultCount += 1;
    }

    public void UseUltimate()
    {
        if (maxUltCount > ultCount)
        {
            ultCount = 0;
            nowUltimate.UseUltimate();
        }
    }
}
