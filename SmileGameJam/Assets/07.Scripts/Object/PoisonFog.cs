using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFog : MonoBehaviour
{
    public int poisonDamge;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            other.GetComponent<UnitInfo>().EnterPoison(poisonDamge);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            other.GetComponent<UnitInfo>().ExitPoison();
    }
}
