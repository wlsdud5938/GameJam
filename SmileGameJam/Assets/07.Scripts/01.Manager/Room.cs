using UnityEngine;
using MapEditor;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public GameObject[] enterances,walls;
    private bool[] openWalls = new bool[4];
    public List<MonsterData> monsters = new List<MonsterData>();

    public bool isEntered = false, isCleared = false;

    public int nowStage = 1;
    public int nowWave = 0;
    public int monsterCount;

    private int obstacleCount;

    public void Open(int r)
    {
        enterances[r].SetActive(true);
        walls[r].SetActive(false);
        openWalls[r] = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCleared && other.CompareTag("Player"))
            EnterTheRoom();
    }

    private void Awake()
    {
        obstacleCount = ObjectData.instance.obstacleList.Length;
    }

    private void Update()
    {
        if (isEntered)
        {
            if(monsterCount <= 0)
            {
                for(int i = 0; i < monsters.Count; i++)
                {
                    Debug.Log(monsters[i].wave);
                    if(monsters[i].wave == nowWave)
                    {
                        Monster newMonster = Instantiate(ObjectData.instance.monsterList[monsters[i].index - obstacleCount], 
                            new Vector3(monsters[i].x, 0, monsters[i].z)+ transform.GetChild(1).transform.position, Quaternion.Euler(0, 90 * monsters[i].rotation, 0));
                        newMonster.SetInfo(nowStage, this);
                        monsterCount++;
                    }
                }
                if (nowWave++ > 3)
                    ClearTheRoom();
            }
        }
    }

    public void EnterTheRoom()
    {
        isEntered = true;
        monsterCount = 0;

        for(int i = 0; i < 4; i++)
        {
            if (openWalls[i])
                walls[i].SetActive(true);
        }
        Debug.Log("I must...");
    }

    public void ClearTheRoom()
    {
        isEntered = false;
        isCleared = true;

        for (int i = 0; i < 4; i++)
        {
            if (openWalls[i])
                walls[i].SetActive(false);
        }
        Debug.Log("ClearTheRoom");
    }
}