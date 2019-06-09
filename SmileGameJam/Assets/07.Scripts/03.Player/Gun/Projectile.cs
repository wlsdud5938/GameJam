using System.Collections;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private IDamageable owner;
    public bool isEnemy;

    public int damage = 0;
    public float range = 5, speed = 10;

    public ParticleSystem[] particles;
    public string id;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        range -= speed * Time.deltaTime;

        if (range <= 0)
            PushToPool();
    }

    public void SetInformation(Player owner, int damage, float speed , float range)
    {
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        this.owner = owner;
        foreach (ParticleSystem p in particles)
        {
            p.Play();
            Debug.Log(p);
        }
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
        ParticlePooler.instance.ReuseObject(id, transform.position, Quaternion.identity, 5);
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

        if (other.CompareTag("Obstacle") || other.CompareTag("Wall"))
        {
            PushToPool();
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Obstacle") || other.transform.CompareTag("Wall"))
        {
            PushToPool();
        }
    }
}
