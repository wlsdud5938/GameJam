using System.Collections;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private IDamageable owner;
    public bool isEnemy;

    public int damage = 0;
    public float range = 5, speed = 10;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        range -= speed * Time.deltaTime;

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
        if (isEnemy)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Player>().TakeDamage(owner, damage);
                PushToPool();
            }
        }
        else
        {
            if (other.CompareTag("Monster"))
            {
                other.GetComponent<Monster>().TakeDamage(owner, damage);
                PushToPool();
            }
        }

        if (other.CompareTag("Wall"))
        {
            PushToPool();
        }
    }
}
