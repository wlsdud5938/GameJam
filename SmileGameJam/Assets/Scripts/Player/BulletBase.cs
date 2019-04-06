using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour{

    public int power = 0;
    public float range = 5, speed = 10;

    protected virtual void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        range -= speed * Time.deltaTime;
        if (range <= 0)
            PushToPool();
    }

    public virtual void SetInformation(int power, float range)
    {
        this.power = power;
        this.range = range;
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
