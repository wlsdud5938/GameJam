using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBush : MonoBehaviour {
    Material material;
    int i;
    Color color;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(2).gameObject.SetActive(false);
        }
        if (other.tag == "Player")
        {
            other.transform.Find("char").transform.GetChild(0).gameObject.SetActive(false);
            other.transform.Find("char").transform.GetChild(1).gameObject.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.GetChild(0).gameObject.SetActive(true);
            other.transform.GetChild(2).gameObject.SetActive(true);
        }
        if (other.tag == "Player")
        {
            other.transform.Find("char").transform.GetChild(0).gameObject.SetActive(true);
            other.transform.Find("char").transform.GetChild(1).gameObject.SetActive(false);
        }
    }





}
