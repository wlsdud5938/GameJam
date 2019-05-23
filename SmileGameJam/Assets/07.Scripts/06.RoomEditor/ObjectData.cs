using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public GameObject[] obstacleList;
    public Monster[] monsterList;
    public GunBase[] gunList;

    public BulletBase playerBullet;

    static ObjectData _instance;
    public static ObjectData instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ObjectData>();
                if (_instance == null)
                    Debug.LogError("There's no ObjectData");
            }
            return _instance;
        }
    }
}