using System.Collections;
using System;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour{

    public int damage = 0;
    public float range = 5, speed = 10;
    private Action hitCall;

    protected virtual void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        range -= speed * Time.deltaTime;
        if (range <= 0)
            PushToPool();
    }

    public virtual void SetInformation(int damage, float speed , float range, Action hitCall)
    {
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        this.hitCall = hitCall;
    }

    public void Reuse(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
    }

    public void PushToPool()
    {
        BulletPooler.instance.PushToPool(this);
    }
}
