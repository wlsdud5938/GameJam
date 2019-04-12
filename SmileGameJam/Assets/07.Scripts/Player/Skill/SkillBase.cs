using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public BulletBase nowBullet;

    public int poolSize = 100;
    protected string id;

    [Header("Information")]
    public int bulletCount = 10;
    public int damage = 5;
    public float range = 6;
    public float bulletSpeed = 10;

    private void Start()
    {
        id = nowBullet.name;
        BulletPooler.instance.CreatePool(id, nowBullet, poolSize);
    }

    public abstract void ShowRange(int power, Vector3 position, float rotation);
    public abstract void HideRange();
    public abstract void UseSkill(int power, float range, Vector3 position, float rotation, UnitInfo owner);
}
