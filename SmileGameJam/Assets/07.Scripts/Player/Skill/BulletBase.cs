using System.Collections;
using System;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour{

    public int damage = 0;
    public float range = 5, speed = 10;
    private UnitInfo owner;

    protected virtual void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        range -= speed * Time.deltaTime;
        if (range <= 0)
            PushToPool();
    }

    public virtual void SetInformation(int damage, float speed , float range, UnitInfo owner)
    {
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        this.owner = owner;
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<UnitInfo>().TakeDamage(owner, damage);
            owner.playerAttack.ChargeUltimate(damage);
            PushToPool();
        }
        else if(other.CompareTag("Box"))
        {
            other.GetComponent<DestroyBox>().TakeDamage(damage);
            PushToPool();
        }
        else if(other.CompareTag("Wall"))
        {
            PushToPool();
        }
    }
}
