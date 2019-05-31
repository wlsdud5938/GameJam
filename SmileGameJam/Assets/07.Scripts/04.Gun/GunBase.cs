using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rare { 기본, 일반, 희귀, 영웅, 전설}
public enum Shake { 끄기, 반대로, 랜덤으로 }

public abstract class GunBase : MonoBehaviour
{
    protected Rare rare;

    protected int poolSize = 100;
    public string id;

    public bool isBasic = false;
    protected bool canPenetrate = false;

    [HideInInspector]
    public int nowCapacity;
    [HideInInspector]
    public float shotDelay;

    [Header("[Information]")]
    protected int maxCapacity;
    protected int attack;
    protected float bulletSpeed;
    protected float accurancy, range;

    public string Capacity
    {
        get
        {
            return nowCapacity + "/" + maxCapacity;
        }
    }

    [Header("[Shake]")]
    protected Shake shake;
    protected float shakeThrust = 0.3f;

    private Projectile bullet;

    public virtual void Start()
    {
        nowCapacity = maxCapacity;

        bullet = ObjectData.instance.bullet;
        bullet.isEnemy = false;
        bullet.canPenetrate = canPenetrate;
        BulletPooler.instance.CreatePool(id + "Bullet", bullet, poolSize);
    }

    public void Shot(Player owner, Vector3 position, float rotation)
    {
        if (nowCapacity <= 0) return;

        if(!isBasic)
            nowCapacity--;

        UseSkill(owner, position, rotation);
    }

    public void ShotFinish()
    {
        
    }

    protected abstract void UseSkill(Player owner, Vector3 position, float rotation);
}
