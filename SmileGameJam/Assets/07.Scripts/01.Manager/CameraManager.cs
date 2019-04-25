using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Transform target;

    public Vector3 dist,shakeDist;

    static CameraManager _instance;
    public static CameraManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraManager>();
                if (_instance == null)
                    Debug.LogError("There's no CameraManager");
            }
            return _instance;
        }
    }

    public void LateUpdate()
    {
        transform.position = target.position + dist + shakeDist;
    }

    public void ReboundCamera(float power, Vector3 direction)
    {
        StartCoroutine(ReboundAnim(power, direction));
    }

    IEnumerator ReboundAnim(float power, Vector3 direction)
    {
        shakeDist = -direction * power;
        while (Vector3.SqrMagnitude(shakeDist) > 0.01f)
        {
            shakeDist *= 0.5f;
            yield return null;
        }
        shakeDist = Vector3.zero;
    }

    public void ShakeCamera()
    {

    }
}
