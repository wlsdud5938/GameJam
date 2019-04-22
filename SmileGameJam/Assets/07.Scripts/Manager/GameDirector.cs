using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    public float playTime = 0.0f;
    public Text timeText;

    public PlayerAttack player;

    public static GameDirector instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        playTime += Time.deltaTime;
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
