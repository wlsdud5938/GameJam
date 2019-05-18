using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public RectTransform mainButtons;
    public RectTransform medias, settings;
    public GameObject touchToReady, crown;

    private float speed = 7;

    private bool isReady = false;

    public void TouchtoReady()
    {
        if (isReady)
        {
            StartCoroutine(MainButtonPopdown());
            StartCoroutine(MediaButtonPopdown());
            StartCoroutine(SettingButtonPopdown());
            StopCoroutine(MainButtonPopup());
            StopCoroutine(MediaButtonPopup());
            StopCoroutine(SettingButtonPopup());
            touchToReady.SetActive(true);
            crown.SetActive(false);
        }
        else
        {
            StartCoroutine(MainButtonPopup());
            StartCoroutine(MediaButtonPopup());
            StartCoroutine(SettingButtonPopup());
            StopCoroutine(MainButtonPopdown());
            StopCoroutine(MediaButtonPopdown());
            StopCoroutine(SettingButtonPopdown());
            crown.SetActive(true);
        }
        isReady = !isReady;
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ContinueButton()
    {

    }

    #region Popup
    IEnumerator MainButtonPopup()
    {
        while (mainButtons.anchoredPosition.y < 30 - speed)
        {
            mainButtons.Translate(Vector3.up * speed);
            yield return null;
        }
        mainButtons.anchoredPosition = new Vector2(0, 30);
        touchToReady.SetActive(false);
    }

    IEnumerator MediaButtonPopup()
    {
        while (medias.anchoredPosition.y < 5 - speed)
        {
            medias.Translate(Vector3.up * speed);
            yield return null;
        }
        medias.anchoredPosition = new Vector2(0, 5);
    }

    IEnumerator SettingButtonPopup()
    {
        while (settings.anchoredPosition.y > -5 + speed)
        {
            settings.Translate(Vector3.down * speed);
            yield return null;
        }
        settings.anchoredPosition = new Vector2(0, -5);
    }
    #endregion

    #region Popdown
    IEnumerator MainButtonPopdown()
    {
        while (mainButtons.anchoredPosition.y > -70 + speed)
        {
            mainButtons.Translate(Vector3.down * speed);
            yield return null;
        }
        mainButtons.anchoredPosition = new Vector2(0, -70);
    }

    IEnumerator MediaButtonPopdown()
    {
        while (medias.anchoredPosition.y > -105 + speed)
        {
            medias.Translate(Vector3.down * speed);
            yield return null;
        }
        medias.anchoredPosition = new Vector2(0, -105);
    }

    IEnumerator SettingButtonPopdown()
    {
        while (settings.anchoredPosition.y < 95 - speed)
        {
            settings.Translate(Vector3.up * speed);
            yield return null;
        }
        settings.anchoredPosition = new Vector2(0, 95);
    }
    #endregion
}