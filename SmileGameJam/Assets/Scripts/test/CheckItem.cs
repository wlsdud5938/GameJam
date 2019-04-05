using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckItem : MonoBehaviour {
    public bool checkItem = false;
    public int itemIdx = -1;

    public Transform ReturnItem()
    {
        return gameObject.transform.GetChild(itemIdx).gameObject.transform;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



}
