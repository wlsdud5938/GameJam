using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public float damage = 5;
    public float speed = 12;
    public float range;

	void Start () {
        range = 6 + Random.Range(-0.3f, 0.2f);
        speed = 12 + Random.Range(-1.5f, 1.5f);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        range -= speed * Time.deltaTime;
        if (range <= 0)
            Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<UnitInfo>().healthPoint -= damage;
            Destroy(gameObject);
        }
        else if(other.CompareTag("Wall"))
            Destroy(gameObject);
    }
}
