using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : Singleton<GameDirector>
{
    [Header("Time")]
    public float playTime = 0.0f;
    public float gameEndTime = 15.0f;
    public int playerCount = 3;
    public bool hasKing = false;

    [Header("Fog")]
    public GameObject[] fogList;
    private float nextWaveTime = 5.0f;
    private float waveTerm = 5.0f, secondWaveTerm = 20.0f;
    private int fogWave = 0, secondWave = 3;

    [Header("Player")]
    public UnitInfo myPlayer;

    void Update()
    {
        playTime += Time.deltaTime;
        if(playTime > nextWaveTime && fogWave < 8)
        {
            fogList[fogWave++].SetActive(true);

            if (fogWave == secondWave)
                nextWaveTime += secondWaveTerm;
            else
                nextWaveTime += waveTerm;
        }

        if (hasKing)
        {
            gameEndTime -= Time.deltaTime;
            if (gameEndTime < 0)
            {
                if (myPlayer.isKing)
                    WinTheGame();
                else
                    LoseTheGame();
            }
        }
    }

    public void BeTheKing(UnitInfo player)
    {
        hasKing = true;
        player.BeTheKing();
}

    public void DiePlayer(UnitInfo player)
    {
        playerCount--;
        if (playerCount <= 0)
            WinTheGame();
    }

    private void WinTheGame()
    {
        SceneManager.LoadScene("WinScene");
    }

    private void LoseTheGame()
    {
        SceneManager.LoadScene("LostScene");
    }
}
