using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public float itemTime = 5.0f;
    public GameObject spawnPoint;
    public GameObject item;
    public Queue<GameObject> itemList;

    int i;
    int idx;
	// Use this for initialization
	void Start () {
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
	}
	
	// Update is called once per frame
	void Update () {
		if(itemTime > 0)
        {
            itemTime -= 1.0f * Time.deltaTime;

        }
        else
        {
            itemTime = 5.0f;
            for(i=0;i<spawnPoint.transform.childCount;i++)
            {
                if (!spawnPoint.transform.GetChild(i).GetComponent<CheckItem>().checkItem &&
                    Random.Range(0, 2) != 0)
                {
                    idx = Random.Range(0, 2);
                    spawnPoint.transform.GetChild(i).transform.GetChild(idx).gameObject.SetActive(true);
                    spawnPoint.transform.GetChild(i).GetComponent<CheckItem>().checkItem = true;
                    spawnPoint.transform.GetChild(i).GetComponent<CheckItem>().itemIdx = idx;
                }
            }
            
        }


	}

}
