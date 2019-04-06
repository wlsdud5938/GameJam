using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFog : MonoBehaviour {
    public float poisonDamge = 0.2f;

    float time = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (time > 0.0f)
            time -= 1.0f * Time.deltaTime;
        else
        {
            time = 1.0f;
            other.GetComponent<UnitInfo>().healthPoint -= other.GetComponent<UnitInfo>().maxHealthPoint * poisonDamge;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        time = 1.0f;
    }
}
