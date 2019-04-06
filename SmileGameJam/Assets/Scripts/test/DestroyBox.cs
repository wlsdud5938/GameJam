using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBox : MonoBehaviour {
    public GameObject brokenBox;
    bool isBroken = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int damage)
    {
        if (isBroken) return;
        isBroken = true;
        GameObject obj =  Instantiate(brokenBox, gameObject.transform.position, Quaternion.identity);
        Destroy(obj, 2.0f);
        Destroy(gameObject);
    }
}
