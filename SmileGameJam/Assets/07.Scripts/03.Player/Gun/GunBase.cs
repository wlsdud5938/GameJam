using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rare { 기본, 일반, 희귀, 영웅, 전설}
public enum Shake { 끄기, 반대로, 랜덤으로 }

public abstract class GunBase : MonoBehaviour
{
    public Rare rare;
    public BulletBase nowBullet;

    public bool isBasic = false;

    public int poolSize = 100;
    public string id;

    [Header("[Information]")]
    public int attack = 5;
    public int maxCapacity = 10, nowCapacity = 10;
    public float bulletSpeed = 10;
    public float shotDelay = 5.0f;
    public float accurancy, range = 6;

    [Header("[Shake]")]
    public Shake shake;
    public float shakeThrust = 0.3f;

    private void Start()
    {
        nowBullet = ObjectData.instance.playerBullet;
        id = this.name;
        BulletPooler.instance.CreatePool(id, nowBullet, poolSize);

        nowCapacity = maxCapacity;
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
