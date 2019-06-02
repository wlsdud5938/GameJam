using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : MonoBehaviour {

    public int index;

    private float delay = 0.5f;
    private Rigidbody rb;
    private BoxCollider[] col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponents<BoxCollider>();
    }

    private void Update()
    {
        if (delay > 0)
            delay -= Time.deltaTime;
        else
        {
            if (rb.velocity.y == 0)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
                col[0].enabled = true;
            }
        }
    }
}