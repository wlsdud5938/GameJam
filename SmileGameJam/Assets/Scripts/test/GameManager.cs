using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public float playTime = 0.0f;
    public bool king = false;
    public bool imKing = false;
    public float gameEndTime = 15.0f;
    public int enemyCount = 3;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if(king || imKing)
        {
            gameEndTime -= 1.0f * Time.deltaTime;
        }
        if (gameEndTime < 0)
        {
            if (imKing)
            {
                SceneManager.LoadScene("WinScene");
            }
            else
                SceneManager.LoadScene("LossScene");
        }
        if(enemyCount <= 0)
            SceneManager.LoadScene("WinScene");
    }

}
