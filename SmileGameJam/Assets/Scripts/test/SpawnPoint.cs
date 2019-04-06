using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    public float itemTime = 10.0f;
    public int itemCount = 4;
    Queue<int> itemList = new Queue<int>();
    int total = 0;
    int i;
    int r=0;
    int s = 0;
    int itemIdx = 0;
    bool firstItem = false;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (itemTime > 0)
        {
            itemTime -= 1.0f * Time.deltaTime;

        }
        else
        {
            itemTime = 10.0f;
            if (!firstItem)
            {
                firstItem = true;
                for (i = 0; i < gameObject.transform.GetChild(0).gameObject.transform.childCount; i++)
                    gameObject.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                i = 0;
                while (itemCount>0)
                {
                    i++;
                    if (i > gameObject.transform.GetChild(1).childCount-1)
                        i = 0;
                    r = Random.Range(0, 3);
                    if (r == 0)
                    {
                        s = Random.Range(0, 4);
                        gameObject.transform.GetChild(1).gameObject.transform.GetChild(i).gameObject.transform.GetChild(s).gameObject.SetActive(true);
                        itemCount--;
                    }
                }

            }

        }
    }
}
