using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePooler : MonoBehaviour
{
    public Dictionary<string, Queue<ParticleSystem>> poolDictionary = new Dictionary<string, Queue<ParticleSystem>>();
    private Dictionary<string, Transform> parentDictionary = new Dictionary<string, Transform>();

    static ParticlePooler _instance;
    public static ParticlePooler instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ParticlePooler>();
                if (_instance == null)
                    Debug.LogError("There's no ParticlePooler");
            }
            return _instance;
        }
    }

    public void CreatePool(string name, ParticleSystem prefab, int size)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            poolDictionary.Add(name, new Queue<ParticleSystem>());
            parentDictionary.Add(name, new GameObject(name + "Pool").transform);
        }

        for (int i = 0; i < size; i++)
        {
            ParticleSystem newObject = Instantiate(prefab, parentDictionary[name]);
            newObject.name = name;
            newObject.gameObject.SetActive(false);
            poolDictionary[name].Enqueue(newObject);
        }
    }

    public void ReuseObject(string tag, Vector3 position, Quaternion rotation, float duration)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            if(poolDictionary[tag].Count < 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    ParticleSystem newObject = Instantiate(poolDictionary[tag].Peek(), parentDictionary[name]);
                    newObject.name = name;
                    newObject.gameObject.SetActive(false);
                    poolDictionary[name].Enqueue(newObject);
                }
            }
            ParticleSystem objectToReuse = poolDictionary[tag].Dequeue();
            objectToReuse.gameObject.SetActive(true);
            objectToReuse.transform.position = position;
            objectToReuse.transform.rotation = rotation;
            objectToReuse.Play();
            StartCoroutine(PushToPool(objectToReuse, duration));
        }
        else
            Debug.LogError("There is no particle : " + tag);
    }

    IEnumerator PushToPool(ParticleSystem obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        poolDictionary[obj.name].Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
}

