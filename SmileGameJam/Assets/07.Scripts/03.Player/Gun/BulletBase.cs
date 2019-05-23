using System.Collections;
using System;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour{

    public int damage = 0;
    public float range = 5, speed = 10;
    private Player owner;

    protected virtual void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        range -= speed * Time.deltaTime;
        if (range <= 0)
            PushToPool();
    }

    public virtual void SetInformation(Player owner, int damage, float speed , float range)
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
    public void Reuse()
    {
        gameObject.SetActive(true);
    }

    public void PushToPool()
    {
        BulletPooler.instance.PushToPool(this);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<Monster>().TakeDamage(owner, damage);
            PushToPool();
        }
        //else if(other.CompareTag("Box"))
        //{
        //    other.GetComponent<DestroyBox>().TakeDamage(damage);
        //    PushToPool();
        //}
        else if(other.CompareTag("Wall"))
        {
            PushToPool();
        }
    }
}
