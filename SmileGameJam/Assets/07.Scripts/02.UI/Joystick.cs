﻿using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; //tnwj
using System;

[Serializable]
public class JoystickStay : UnityEvent<float, float> { }
[Serializable]
public class JoystickUp : UnityEvent<bool> { }

public class Joystick : MonoBehaviour
{
    private const float radius = 65;
    #region Private Field
    private RectTransform joystick;
    private RectTransform lever;

    private Vector2 disabledPos, firstPos, nowPos;
    private bool isStick_Stay = false, isMoved = false;
    #endregion

    public bool isLeftField = false;
    public bool isActive = true;

    private Camera uiCamera;

    #region Protected Field
    protected float distance;
    protected Vector3 direction;
    protected float rotation
    {
        get
        {
            return Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        }
    }
    #endregion

    #region Call Event
    public UnityEvent GetJoystickDown;
    public JoystickStay GetJoystickStay;
    public JoystickUp GetJoystickUp;
    #endregion

    #region Event Function
    protected virtual void Start()
    {
        uiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
        joystick = transform.GetComponent<RectTransform>();
        lever = joystick.GetChild(0).GetComponent<RectTransform>();
        disabledPos = new Vector3(joystick.anchoredPosition.x, joystick.anchoredPosition.y, 0);
    }

    protected virtual void Update()
    {
        if (!isActive) return;
#if UNITY_EDITOR
        TestMovement();
#endif

#if UNITY_ANDROID
        foreach (Touch touch in Input.touches)
        {
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId)
                && ((!isLeftField && touch.position.x > Screen.width * 0.5f)
                || (isLeftField && touch.position.x < Screen.width * 0.5f))
                )
            {
                if (touch.phase == TouchPhase.Began)
                    Start_Move_Joystick(touch.position);
                else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    Stay_Move_Joystick(touch.position);
                else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                    End_Move_Joystick();
                break;
            }
        }//헤해;ㅜ
#endif
    }
    #endregion

    #region UNITY_ANDROID Function
#if UNITY_ANDROID
    private void Start_Move_Joystick(Vector2 touchPos)
    {
        isStick_Stay = true;
        isMoved = false;

        //시작 점, 조이스틱위치 초기화
        firstPos = touchPos;
        joystick.position = UIPosition(touchPos);
        nowPos = firstPos;

        GetJoystickDown.Invoke();
    }

    private void Stay_Move_Joystick(Vector2 touchPos)
    {
        nowPos = touchPos;

        if (Vector2.SqrMagnitude(nowPos - firstPos) > 0.1f) isMoved = true;

        distance = Vector3.Distance(nowPos, firstPos);
        distance = (distance < radius) ? distance : radius;

        nowPos = (nowPos - firstPos).normalized;
        lever.anchoredPosition = nowPos * distance;

        direction = nowPos;
    
        GetJoystickStay.Invoke(distance / radius, rotation);
    }

    private void End_Move_Joystick()
    {
        GetJoystickUp.Invoke(isMoved);

        isStick_Stay = false;

        direction = Vector3.zero;
        distance = 0;

        lever.anchoredPosition = Vector3.zero;
        joystick.anchoredPosition = disabledPos;
    }
#endif
    #endregion

    #region UNITY_EDITOR Function
#if UNITY_EDITOR
    private void TestMovement()
    {
        if (!EventSystem.current.IsPointerOverGameObject() &&
            ((!isLeftField && Input.GetMouseButtonDown(1)) ||
            (isLeftField && Input.GetMouseButtonDown(0))))
        {
            isStick_Stay = true;
            isMoved = false;

            //시작 점, 조이스틱위치 초기화
            firstPos = Input.mousePosition;
            joystick.position = UIPosition(Input.mousePosition);
            nowPos = firstPos;

            GetJoystickDown.Invoke();
        }
        else if (isStick_Stay && ((!isLeftField && Input.GetMouseButtonUp(1)) ||
            (isLeftField && Input.GetMouseButtonUp(0))))
        {
            GetJoystickUp.Invoke(isMoved);

            isStick_Stay = false;

            direction = Vector3.zero;
            distance = 0;

            lever.anchoredPosition = Vector3.zero;
            joystick.anchoredPosition = disabledPos;
        }
        else if (isStick_Stay)
        {
            nowPos = Input.mousePosition;

            if (Vector2.SqrMagnitude(nowPos - firstPos) > 0.1f) isMoved = true;

            distance = Vector3.Distance(nowPos, firstPos);
            distance = (distance < radius) ? distance : radius;

            nowPos = (nowPos - firstPos).normalized;
            lever.anchoredPosition = nowPos * distance;

            direction = nowPos;

            GetJoystickStay.Invoke(distance / radius, rotation);
        }
    }
#endif
    #endregion

    public void JoystickActive(bool on)
    {
        if (on)
        {
            joystick.GetComponent<Image>().color = new Color(1,1,1,1);
            lever.GetComponent<Image>().color = new Color(1, 1, 1, 0.62f);
        }
        else
        {
            joystick.GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
            lever.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
        }
        isActive = on;
    }

    private Vector3 UIPosition(Vector3 position)
    {
        var screenPoint = new Vector3(position.x, position.y, 100.0f);
        return uiCamera.ScreenToWorldPoint(screenPoint);
    }
}
