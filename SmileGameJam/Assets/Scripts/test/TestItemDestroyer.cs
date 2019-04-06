using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemDestroyer : MonoBehaviour {
    public int point = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<UnitInfo>().score += point;
        other.GetComponent<UnitInfo>().maxHealthPoint += other.GetComponent<UnitInfo>().maxHealthPoint * point * 0.1f;
        gameObject.transform.parent.GetComponent<CheckItem>().checkItem = false;
        gameObject.SetActive(false);
    }
    
}
