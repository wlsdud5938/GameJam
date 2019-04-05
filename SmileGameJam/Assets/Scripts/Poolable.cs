using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour {

    public string id;

    public void Reuse(Vector3 position, Quaternion rotation)
    {
        BulletPooler.instance.ReuseObject(id, position, rotation);
    }

    public void PushToPool()
    {
        BulletPooler.instance.PushToPool(this);
    }
}
