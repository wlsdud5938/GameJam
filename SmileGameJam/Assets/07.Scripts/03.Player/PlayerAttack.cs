﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : JoystickBase
{
    [Header("Muzzle")]
    public Transform muzzle;
    private float rot = 0;
    private float muzzleRot
    {
        set
        {
            rot = value;
        }
        get
        {
            return rot + 90;
        }
    }

    [Header("Gun")]
    public int index = 0, maxCount = 2;
    public GunBase nowGun;
    public GunBase[] gunInventory;

    [Header("Gun Info")]
    public Image gunImage;
    public Text bulletCountText;
    private float nowTerm = 0, bulletTerm;

    private Animator animator;
    private Player player;

    private void Start()
    {
        player = transform.GetComponent<Player>();
        animator = transform.GetComponent<Animator>();
        SetGunInfo();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SwitchGun()
    {
        index = index + 1 >= maxCount ? 0 : index + 1;
        nowGun = gunInventory[index];

        SetGunInfo();
    }

    public void GetGun(int id)
    {
        //총 잡기
        //gunInventory[1] = gunData[id];

        maxCount++;

        SetGunInfo();
    }

    public void RemoveGun()
    {
        index = 0;
        nowGun = gunInventory[index];

        Destroy(gunInventory[1].gameObject);
        gunInventory[1] = null;

        maxCount--;

        SetGunInfo();
    }



    public void SetGunInfo()
    {
        nowTerm = bulletTerm = nowGun.bulletTerm;
        //이미지 교체
        SetGunUI();
    }

    public void SetGunUI()
    {
        if (nowGun.isBasic)
            bulletCountText.text = "∞";
        else
            bulletCountText.text = nowGun.nowBulletCount + "/" + nowGun.maxBulletCount;
    }

    protected override void GetJoystickDown()
    {
        muzzleRot = rotation;
    }

    protected override void GetJoystickStay(float dist)
    {
        muzzleRot = rotation;

        if (nowTerm <= 0)
        {
            nowGun.Shot(player, muzzle.position, rotation);
            SetGunUI();

            if (nowGun.nowBulletCount <= 0)
                RemoveGun();

            nowTerm = bulletTerm;
        }
        else
            nowTerm -= Time.deltaTime;
    }

    protected override void GetJoystickUp(bool isClicked)
    {
        muzzleRot = 0;
        nowTerm = bulletTerm;
    }

    private void LateUpdate()
    {
        muzzle.eulerAngles = new Vector3(0, muzzleRot, 0);
    }
}