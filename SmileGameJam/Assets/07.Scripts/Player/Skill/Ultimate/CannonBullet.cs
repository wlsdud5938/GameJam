using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    private UnitInfo owner;
    public int damage = 0;
    public float range = 5, speed = 10;

    public float radius = 2;

    protected virtual void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        range -= speed * Time.deltaTime;
        if (range <= 0)
            Destroy(gameObject);
    }

    public virtual void SetInformation(int damage, float speed, float range, UnitInfo owner)
    {
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        this.owner = owner;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<UnitInfo>().TakeDamage(owner, damage);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(other.gameObject);
        }
    }
}