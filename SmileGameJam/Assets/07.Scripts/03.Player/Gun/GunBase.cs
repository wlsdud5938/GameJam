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
    public bool isBasic = false;

    public float bulletTerm = 5.0f;
    public float bulletSpeed = 10;
    public float bulletRange = 6;

    public int nowBulletCount, maxBulletCount = 10;
    public int damage = 5;

    private void Start()
    {
        id = nowBullet.name;
        BulletPooler.instance.CreatePool(id, nowBullet, poolSize);

        nowBulletCount = maxBulletCount;
    }

    public void Shot(Player owner, Vector3 position, float rotation)
    {
        if (nowBulletCount <= 0)
            return;

        if(!isBasic)
            nowBulletCount--;

        UseSkill(owner, position, rotation);
    }

    protected abstract void UseSkill(Player owner, Vector3 position, float rotation);
}
