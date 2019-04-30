using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour {

    private Camera mainCam;

    [Header("Camera Move")]
    public float moveSpeed = 0.07f;
    private Vector3 previousMousePos;
    
    public float heightSpeed = 0.1f;
    private float height = 17;
    private float minHeight = 1, maxHeight = 20;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
            previousMousePos = Input.mousePosition;
        else if (Input.GetMouseButton(1))
        {
            float movePosX = previousMousePos.x - Input.mousePosition.x;
            float movePosY = previousMousePos.y - Input.mousePosition.y;

            if (Mathf.Abs(movePosX) < 0.5f) movePosX = 0;
            if (Mathf.Abs(movePosY) < 0.5f) movePosY = 0;


            mainCam.transform.Translate(new Vector3(movePosX, movePosY) * moveSpeed);
            previousMousePos = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.mouseScrollDelta.y < 0)
                height = Mathf.Clamp(height + heightSpeed, minHeight, maxHeight);
            else if (Input.mouseScrollDelta.y > 0)
                height = Mathf.Clamp(height - heightSpeed, minHeight, maxHeight);

            mainCam.transform.position = new Vector3(mainCam.transform.position.x, height, mainCam.transform.position.z);
        }
    }
}
