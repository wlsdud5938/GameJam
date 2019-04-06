using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    public float itemTime = 30.0f;
    public List<int> itemCount;
    Queue<int> itemList = new Queue<int>();
    int total = 0;
    int i;
    int idx=0;
    int itemIdx = 0;

    // Use this for initialization
    void Start () {
        for (i = 0; i < itemCount.Count; i++)
            total += itemCount[i];
        while(true)
        {
            if (itemCount[idx] != 0)
            {
                itemList.Enqueue(idx);
                itemCount[idx]--;
                total--;
            }
            idx++;
            if (idx == 4)
                idx = 0;
            if (total == 0)
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (itemTime > 0)
        {
            itemTime -= 1.0f * Time.deltaTime;

        }
        else
        {
            itemTime = 5.0f;
            while (true)
            {
                if (itemList.Count == 0)
                    break;
                for(i=0;i<gameObject.transform.childCount;i++)
                {
                    idx = Random.Range(0, 2);
                    if (idx != 0 && !gameObject.transform.GetChild(i).GetComponent<CheckItem>().checkItem)
                    {
                        itemIdx = itemList.Dequeue();
                        gameObject.transform.GetChild(i).transform.GetChild(itemIdx).gameObject.SetActive(true);
                        gameObject.transform.GetChild(i).GetComponent<CheckItem>().itemIdx = itemIdx;
                        gameObject.transform.GetChild(i).GetComponent<CheckItem>().checkItem = true;
                    }
                    if (itemList.Count == 0)
                        break;
                }

            }

        }
    }
}
