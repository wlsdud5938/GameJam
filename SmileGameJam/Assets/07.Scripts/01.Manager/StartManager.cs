using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public RectTransform mainButtons;
    public RectTransform medias, settings;
    public GameObject touchToReady, crown;

    public Animation anim;

    private float speed = 18f;

    private bool isReady = false;

    public void TouchtoReady()
    {
        if (isReady)
        {
            StopAllCoroutines();
            StartCoroutine(MainButtonPopdown());
            StartCoroutine(MediaButtonPopdown());
            StartCoroutine(SettingButtonPopdown());
            touchToReady.SetActive(true);
            crown.SetActive(false);
            //anim.Play("ShowDown");
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(MainButtonPopup());
            StartCoroutine(MediaButtonPopup());
            StartCoroutine(SettingButtonPopup());
            touchToReady.SetActive(false);
            crown.SetActive(true);
            //anim.Play("ShowUp");
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
        mainButtons.anchoredPosition = new Vector2(0, -70);
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
        medias.anchoredPosition = new Vector2(0, -105);
        while (medias.anchoredPosition.y < 5 - speed)
        {
            medias.Translate(Vector3.up * speed);
            yield return null;
        }
        medias.anchoredPosition = new Vector2(0, 5);
    }

    IEnumerator SettingButtonPopup()
    {
        settings.anchoredPosition = new Vector2(0, 95);
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
        mainButtons.anchoredPosition = new Vector2(0, 30);
        while (mainButtons.anchoredPosition.y > -70 + speed)
        {
            mainButtons.Translate(Vector3.down * speed);
            yield return null;
        }
        mainButtons.anchoredPosition = new Vector2(0, -70);
    }

    IEnumerator MediaButtonPopdown()
    {
        medias.anchoredPosition = new Vector2(0, 5);
        while (medias.anchoredPosition.y > -105 + speed)
        {
            medias.Translate(Vector3.down * speed);
            yield return null;
        }
        medias.anchoredPosition = new Vector2(0, -105);
    }

    IEnumerator SettingButtonPopdown()
    {
        settings.anchoredPosition = new Vector2(0, -5);
        while (settings.anchoredPosition.y < 95 - speed)
        {
            settings.Translate(Vector3.up * speed);
            yield return null;
        }
        settings.anchoredPosition = new Vector2(0, 95);
    }
    #endregion
}