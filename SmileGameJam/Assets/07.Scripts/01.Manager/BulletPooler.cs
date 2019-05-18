using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    public Dictionary<string, Queue<BulletBase>> poolDictionary = new Dictionary<string, Queue<BulletBase>>();
    private Dictionary<string, Transform> parentDictionary = new Dictionary<string, Transform>();

    static BulletPooler _instance;
    public static BulletPooler instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BulletPooler>();
                if (_instance == null)
                    Debug.LogError("There's no BulletPooler");
            }
            return _instance;
        }
    }

    public void CreatePool(string name, BulletBase prefab, int size)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            poolDictionary.Add(name, new Queue<BulletBase>());
            parentDictionary.Add(name, new GameObject(name + "Pool").transform);
        }

        for (int i = 0; i < size; i++)
        {
            BulletBase newObject = Instantiate(prefab, parentDictionary[name]);
            newObject.name = name;
            newObject.gameObject.SetActive(false);
            poolDictionary[name].Enqueue(newObject);
        }
    }

    public BulletBase ReuseObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            BulletBase objectToReuse = poolDictionary[tag].Dequeue();
            objectToReuse.Reuse(position, rotation);
            return objectToReuse;
        }
        Debug.LogError("There is no particle : " + tag);
        return null;
    }

    public BulletBase ReuseObject(string tag)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            BulletBase objectToReuse = poolDictionary[tag].Dequeue();
            objectToReuse.Reuse();
            return objectToReuse;
        }
        Debug.LogError("There is no particle : " + tag);
        return null;
    }

    public void PushToPool(BulletBase obj)
    {
        poolDictionary[obj.name].Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
}

