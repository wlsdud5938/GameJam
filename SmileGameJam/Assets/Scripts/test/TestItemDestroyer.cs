using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemDestroyer : MonoBehaviour {
    public int point = 1;
    public GameObject spawnItem;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            if (point >= 2)
                gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.GetComponent<SpawnPoint>().itemCount++;
            other.GetComponent<UnitInfo>().score += point;
            other.GetComponent<UnitInfo>().maxHealthPoint += (int)(other.GetComponent<UnitInfo>().maxHealthPoint * point * 0.1f);
            gameObject.transform.parent.GetComponent<CheckItem>().checkItem = false;
            gameObject.SetActive(false);
        }
    }
    
}
