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
            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        if (other.tag == "Player")
        {
            for (i = 0; i < other.transform.Find("Model").GetComponent<MeshRenderer>().materials.Length; i++)
            {
                material = other.transform.Find("Model").GetComponent<MeshRenderer>().materials[i];
                color = material.color;
                color.a = 0.5f;
                material.color = new Color(color.r, color.g, color.b, 0.7f);
                Debug.Log(other.transform.Find("Model").GetComponent<MeshRenderer>().material.color.a);
                //other.transform.Find("Model").GetComponent<MeshRenderer>().materials[i] = material;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    }





}
