using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemDestroyer : MonoBehaviour {

    public int point = 1;
    public GameObject spawnItem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            if (point >= 2)
                gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.GetComponent<SpawnPoint>().itemCount++;
            other.GetComponent<UnitInfo>().GetScore(point);
            gameObject.transform.parent.GetComponent<CheckItem>().checkItem = false;
            gameObject.SetActive(false);
        }
    }
}