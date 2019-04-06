using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Transform target;

    public Vector3 dist;

    public void LateUpdate()
    {
        transform.position = target.position + dist;
    }
}
