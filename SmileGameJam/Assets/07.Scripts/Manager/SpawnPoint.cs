using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float itemTime = 10.0f;
    public int itemCount = 4;
    bool firstItem = false;

    void Update()
    {
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
                for (int i = 0; i < gameObject.transform.GetChild(0).gameObject.transform.childCount; i++)
                    gameObject.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                int i = 0;
                while (itemCount > 0)
                {
                    i++;
                    if (i > gameObject.transform.GetChild(1).childCount - 1)
                        i = 0;
                    int r = Random.Range(0, 3);
                    if (r == 0)
                    {
                        gameObject.transform.GetChild(1).gameObject.transform.GetChild(i).gameObject.transform.GetChild(Random.Range(0, 4)).gameObject.SetActive(true);
                        itemCount--;
                    }
                }

            }

        }
    }
}
