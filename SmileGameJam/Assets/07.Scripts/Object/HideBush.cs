using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBush : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (other.tag == "Player")
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.GetChild(0).gameObject.SetActive(true);
            other.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (other.tag == "Player")
        {
            other.transform.GetChild(0).gameObject.SetActive(true);
            other.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
