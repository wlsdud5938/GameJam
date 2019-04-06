using System.Collections;
using System;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour{

    public int damage = 0;
    public float range = 5, speed = 10;
    private PlayerAttack owner;

    protected virtual void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        range -= speed * Time.deltaTime;
        if (range <= 0)
            PushToPool();
    }

    public virtual void SetInformation(int damage, float speed , float range, PlayerAttack owner)
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
            other.GetComponent<UnitInfo>().TakeDamage(damage);
            owner.ChargeUltimate(damage);
            PushToPool();
        }
    }
}
