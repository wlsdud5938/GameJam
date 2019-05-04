using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text timeText;

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    IEnumerator BoomText(int start)
    {
        Transform timeTr = timeText.transform;
        for (int i = start; i < 10; i++)
        {
            timeTr.localScale = Vector3.one * 0.1f * i;
            yield return null;
        }
        yield return new WaitForSeconds(1);
    }
}
