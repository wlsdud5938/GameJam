using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    public Dictionary<string, Queue<Poolable>> poolDictionary = new Dictionary<string, Queue<Poolable>>();
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

    public void CreatePool(string name, GameObject prefab, int size)
    {
        poolDictionary.Add(tag, new Queue<Poolable>());
        parentDictionary.Add(tag, new GameObject(tag + "Pool").transform);

        for (int i = 0; i < size; i++)
        {
            Poolable newObject = Instantiate(prefab, parentDictionary[tag]).GetComponent<Poolable>();
            newObject.gameObject.SetActive(false);
            poolDictionary[tag].Enqueue(newObject);
        }
    }

    public Poolable ReuseObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Poolable objectToReuse = poolDictionary[tag].Dequeue();
            objectToReuse.transform.position = position;
            objectToReuse.transform.rotation = rotation;
            objectToReuse.gameObject.SetActive(true);
            return objectToReuse;
        }
        Debug.LogError("There is no particle : " + tag);
        return null;
    }

    public void PushToPool(Poolable obj)
    {
        poolDictionary[obj.id].Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
}

