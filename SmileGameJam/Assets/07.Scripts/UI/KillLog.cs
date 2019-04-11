using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KillLog : MonoBehaviour
{
    private Animation anim;
    private int startPos = 345, endPos = 545;

    public Image killingImage, killedImage;
    public Text killingText, killedText;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void ShowLog(UnitInfo killing, UnitInfo killed)
    {
        anim.Play("ShowAnim");
        killingImage.sprite = killing.characterImage;
        killingText.text = killing.nickname;
        killedImage.sprite = killed.characterImage;
        killedText.text = killed.nickname;
    }

    public void HideLog()
    {
        anim.Play("HideAnim");
    }
}