﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresueChest : MonoBehaviour {

    public GameObject hasItem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            Rigidbody newItem = Instantiate(hasItem, transform.position + Vector3.up * 0.5f, Quaternion.identity).GetComponent<Rigidbody>();
            newItem.AddForce(Vector3.up * 5, ForceMode.Impulse);
            //newItem.AddForce(new Vector3(Random.Range(-2.0f, 2.0f), 5, -2), ForceMode.Impulse);

            Destroy(gameObject);
        }
    }
}
