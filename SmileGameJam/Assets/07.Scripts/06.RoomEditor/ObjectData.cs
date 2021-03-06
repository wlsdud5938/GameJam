﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public GameObject[] obstacleList;
    public GameObject[] monsterList;
    public GunBase[] gunList;
    public ItemCtrl[] itemList;
    public Projectile[] bulletList;
    public ParticleSystem[] bulletParticleList;

    public Dictionary<string, GameObject> obstacles = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> monsters = new Dictionary<string, GameObject>();
    public Dictionary<string, GunBase> guns = new Dictionary<string, GunBase>();
    public Dictionary<string, ItemCtrl> items = new Dictionary<string, ItemCtrl>();
    public Dictionary<string, Projectile> bullets = new Dictionary<string, Projectile>();

    public bool isEditor = false;

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
        foreach (GameObject m in monsterList)
            monsters.Add(m.name, m);
        if (!isEditor)
        {
            foreach (GunBase g in gunList)
                guns.Add(g.name, g);
            foreach (ItemCtrl i in itemList)
                items.Add(i.name, i);
            foreach (Projectile p in bulletList)
                bullets.Add(p.name, p);
            foreach (ParticleSystem p in bulletParticleList)
                ParticlePooler.instance.CreatePool(p.name, p, 50);
        }
    }
}