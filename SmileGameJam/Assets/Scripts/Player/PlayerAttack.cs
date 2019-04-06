using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {

    public Transform muzzle;
    private PlayerMove playerMove;

    [Header("Skill")]
    public BulletBase nowBullet;
    public int poolSize = 15;
    public float range = 6;
    private string id;
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
        UseSkill();
        isClicked = false;
        percent = index = 0;
        chargeBar.fillAmount = 0;
    }

    private void Start()
    {
        id = nowBullet.name;
        BulletPooler.instance.CreatePool(id, nowBullet, poolSize);

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
        }
    }

    public void UseSkill()
    {
        BulletBase newBullet = 
            BulletPooler.instance.ReuseObject(id, transform.position + playerMove.targetRot * Vector3.forward * 0.2f, playerMove.targetRot);
        newBullet.SetInformation(index, range);
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
