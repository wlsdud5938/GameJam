using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    public int damage = 0;
    public float range = 5, speed = 10;

    public float radius = 2;

    protected virtual void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        range -= speed * Time.deltaTime;
        if (range <= 0)
            Boom();
    }

    public virtual void SetInformation(int damage, float speed, float range)
    {
        this.damage = damage;
        this.speed = speed;
        this.range = range;
    }

    public void Boom()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in cols)
        {
            if (col.CompareTag("Enemy"))
            {
                col.GetComponent<UnitInfo>().healthPoint -= damage;
            }
            else if (col.CompareTag("Wall"))
            {
                Destroy(col.gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<UnitInfo>().healthPoint -= damage;
            Boom();
        }
        else if (other.CompareTag("Wall"))
        {
            Boom();
        }
    }
}