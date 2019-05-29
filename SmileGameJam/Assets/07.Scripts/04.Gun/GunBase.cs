using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rare { 기본, 일반, 희귀, 영웅, 전설}
public enum Shake { 끄기, 반대로, 랜덤으로 }

public abstract class GunBase : MonoBehaviour
{
    protected Rare rare;
    protected string bulletName;

    protected int poolSize = 100;
    protected string id;

    public bool isBasic = false;

    [Header("[Information]")]
    public float shotDelay = 0.5f;
    public int maxCapacity = 10, nowCapacity = 10;
    protected int attack = 5;
    protected float bulletSpeed = 10;
    protected float accurancy, range = 6;

    [Header("[Shake]")]
    protected Shake shake;
    protected float shakeThrust = 0.3f;

    private Projectile bullet;

    private void Start()
    {
        id = name;
        nowCapacity = maxCapacity;

        bullet = ObjectData.instance.bullet;
        bullet.isEnemy = false;
        BulletPooler.instance.CreatePool(id, bullet, poolSize);
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
