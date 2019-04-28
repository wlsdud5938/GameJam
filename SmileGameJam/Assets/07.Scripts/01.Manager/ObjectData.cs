using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour {

    public GameObject[] obstacleList;
    public GameObject[] monsterList;
    public static ObjectData instance;

    private void Awake()
    {
        instance = this;
    }
}
