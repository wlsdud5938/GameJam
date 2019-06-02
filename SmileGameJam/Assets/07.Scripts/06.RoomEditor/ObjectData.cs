using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public GameObject[] obstacleList;
    public Monster[] monsterList;
    public GunBase[] gunList;
    public ItemCtrl[] itemList;

    public Projectile playerBullet;

    public Dictionary<string, GameObject> obstacles = new Dictionary<string, GameObject>();
    public Dictionary<string, Monster> monsters = new Dictionary<string, Monster>();
    public Dictionary<string, GunBase> guns = new Dictionary<string, GunBase>();
    public Dictionary<string, ItemCtrl> items = new Dictionary<string, ItemCtrl>();

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
    private void Awake()
    {
        foreach (GameObject o in obstacleList)
            obstacles.Add(o.name, o);
        foreach (Monster m in monsterList)
            monsters.Add(m.name, m);
        foreach (GunBase g in gunList)
            guns.Add(g.name, g);
        foreach (ItemCtrl i in itemList)
            items.Add(i.name, i);
    }
}