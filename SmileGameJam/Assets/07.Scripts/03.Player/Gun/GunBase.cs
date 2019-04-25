using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunBase : MonoBehaviour
{
    public BulletBase nowBullet;

    public int poolSize = 100;
    protected string id;

    [Header("Information")]
    public float nowTerm = 0, bulletTerm = 5.0f;
    public float bulletSpeed = 10;
    public float bulletRange = 6;
    public int damage = 5;

    private void Start()
    {
        id = nowBullet.name;
        BulletPooler.instance.CreatePool(id, nowBullet, poolSize);
    }

    public void GetGunStay(Player owner, Vector3 position, float rotation)
    {
        if(nowTerm > bulletTerm)
        {
            UseSkill(owner, position, rotation);
            nowTerm = 0;
        }
        else
            nowTerm += Time.deltaTime;
    }

    public void GetGunUp()
    {
        nowTerm = 0;
    }

    protected abstract void UseSkill(Player owner, Vector3 position, float rotation);
}
