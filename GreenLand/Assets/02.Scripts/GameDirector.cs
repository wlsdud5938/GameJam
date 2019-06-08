using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour {

    private float nowTime;
    private float cloudSpeed = 20.0f;

    [Header("[Start Scene]")]
    public bool isStart = false;
    public GameObject touchtoplay;
    public RectTransform startScene;
    public RectTransform title, island;
    public RectTransform[] cloud;

    [Header("[Ingame Scene]")]
    public RectTransform ingameScene;

    void Update()
    {
        nowTime += Time.deltaTime;
        if (!isStart)
        {
            island.anchoredPosition = new Vector2(0, 50 + Mathf.Sin(nowTime * 1.5f) * 20);
            for (int i = 0; i < cloud.Length; i++)
            {
                cloud[i].anchoredPosition += Vector2.left * cloudSpeed * Time.deltaTime;
                if (cloud[i].anchoredPosition.x < -500)
                    cloud[i].anchoredPosition = new Vector2(500, cloud[i].anchoredPosition.y);
            }
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartGameAni());
    }

    IEnumerator StartGameAni()
    {
        isStart = true;
        touchtoplay.SetActive(false);

        float speed = 30;
        while (title.anchoredPosition.y < 135 || island.anchoredPosition.y > -800)
        {
            title.anchoredPosition += Vector2.up * speed;
            island.anchoredPosition += Vector2.down * speed;
            yield return null;
        }

        speed = 50;
        while (startScene.anchoredPosition.x > -800)
        {
            startScene.anchoredPosition += Vector2.left * speed;
            yield return null;
        }
    }
}
