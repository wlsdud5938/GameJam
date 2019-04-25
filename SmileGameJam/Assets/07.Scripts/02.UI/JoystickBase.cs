using UnityEngine.EventSystems;
using UnityEngine;

public abstract class JoystickBase : MonoBehaviour
{
    #region Protected Field
    public RectTransform joystick;
    private RectTransform lever;
    #endregion

    #region Private Field
    private Vector2 disabledPos, firstPos, nowPos;
    private bool isStick_Stay = false;
    private bool isDraged = false;
    private int nowTouch = -1;
    protected float distance { get; private set; }
    protected Vector3 direction { get; private set; }
    protected float rotation
    {
        get
        {
            return Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        }
    }
    #endregion

    public bool isLeftField = false;
    public float radius = 65;

    #region Event Function
    protected virtual void Awake()
    {
        lever = joystick.GetChild(0).GetComponent<RectTransform>();
        disabledPos = new Vector2(joystick.anchoredPosition.x, joystick.anchoredPosition.y);
    }

    protected virtual void Update()
    {
#if UNITY_EDITOR
        TestMovement();
#endif

#if UNITY_ANDROID
        foreach (Touch touch in Input.touches)
        {
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId) &&
                touch.phase == TouchPhase.Began)
            {
                if ((!isLeftField && touch.position.x > Screen.width * 0.5f) ||
                    (isLeftField && touch.position.x < Screen.width * 0.5f))
                    Start_Move_Joystick(touch.fingerId);
            }
        }

        if (isStick_Stay && nowTouch >= 0)
        {
            while (true)
            {
                try {
                    Input.GetTouch(nowTouch);
                    break;
                }
                catch {
                    nowTouch--;
                    if (nowTouch < 0) End_Move_Joystick();
                }
            }

            Debug.Log(Input.GetTouch(nowTouch).phase);
            switch (Input.GetTouch(nowTouch).phase)
            {
                case TouchPhase.Moved:
                    isDraged = true;
                    Stay_Move_Joystick();
                    break;
                case TouchPhase.Began:
                case TouchPhase.Stationary:
                    Stay_Move_Joystick();
                    break;

                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    End_Move_Joystick();
                    break;
            }
        }
#endif
    }
    #endregion

    #region Call Event
    protected abstract void GetJoystickDown();
    protected abstract void GetJoystickStay(float dist);
    protected abstract void GetJoystickUp(bool isClicked);
    #endregion

    #region UNITY_ANDROID Function
#if UNITY_ANDROID
    private void Start_Move_Joystick(int touch)
    {
        nowTouch = touch;
        isStick_Stay = true;
        isDraged = false;
        
        //시작 점, 조이스틱위치 초기화
        firstPos = joystick.position = Input.GetTouch(touch).position;
        nowPos = firstPos;

        GetJoystickDown();
    }

    private void Stay_Move_Joystick()
    {
        nowPos = Input.GetTouch(nowTouch).position;
        
        if (Vector2.SqrMagnitude(nowPos - firstPos) > 0.1f) isDraged = true;

        distance = Vector3.Distance(nowPos, firstPos);
        distance = (distance < radius) ? distance : radius;

        nowPos = (nowPos - firstPos).normalized;
        lever.anchoredPosition = nowPos * distance;

        direction = nowPos;
    
        GetJoystickStay(distance / radius);
    }

    private void End_Move_Joystick()
    {
        GetJoystickUp(!isDraged);

        isStick_Stay = false;
        nowTouch = -1;

        direction = Vector2.zero;
        distance = 0;

        lever.anchoredPosition = Vector2.zero;
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
            isDraged = false;

            //시작 점, 조이스틱위치 초기화
            firstPos = joystick.position = Input.mousePosition;
            nowPos = firstPos;

            GetJoystickDown();
        }
        else if (isStick_Stay && ((!isLeftField && Input.GetMouseButton(1)) ||
                (isLeftField && Input.GetMouseButton(0))))
        {
            nowPos = Input.mousePosition;

            if (Vector2.SqrMagnitude(nowPos - firstPos) > 0.1f) isDraged = true;

            distance = Vector3.Distance(nowPos, firstPos);
            distance = (distance < radius) ? distance : radius;

            nowPos = (nowPos - firstPos).normalized;
            lever.anchoredPosition = nowPos * distance;

            direction = nowPos;

            GetJoystickStay(distance / radius);
        }
        else if (isStick_Stay && ((!isLeftField && Input.GetMouseButtonUp(1)) ||
            (isLeftField && Input.GetMouseButtonUp(0))))
        {
            GetJoystickUp(!isDraged);

            isStick_Stay = false;
            nowTouch = -1;

            direction = Vector2.zero;
            distance = 0;

            lever.anchoredPosition = Vector2.zero;
            joystick.anchoredPosition = disabledPos;
        }
    }
#endif
    #endregion
}
