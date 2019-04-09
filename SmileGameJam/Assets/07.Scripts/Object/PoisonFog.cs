using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFog : MonoBehaviour
{
    public int poisonDamge;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            other.GetComponent<UnitInfo>().ExitPoison();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            other.GetComponent<UnitInfo>().EnterPoison(poisonDamge);
    }
}