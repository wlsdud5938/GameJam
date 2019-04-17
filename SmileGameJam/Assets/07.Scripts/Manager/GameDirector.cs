using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameDirector : Singleton<GameDirector>
{
    [Header("Time")]
    public float playTime = 0.0f;
    public float gameEndTime = 15.0f;
    private int gameNextTime = 16;
    public int playerCount = 3;
    public bool hasKing = false;
    public Text timeText;

    [Header("Fog")]
    public GameObject[] fogList;
    public BoxCollider safeCollider;
    private float safeSize = 50;

    private float nextWaveTime = 0;
    public float waveTerm = 5.0f;
    public float firstWaveTerm = 20.0f, secondWaveTerm = 20.0f;
    private int fogWave = 0, secondWave = 4;

    [Header("Player")]
    public UnitInfo myPlayer;

    private void Start()
    {
        playTime = -firstWaveTerm;
    }

    void Update()
    {
        playTime += Time.deltaTime;
        if (playTime > nextWaveTime && fogWave < 8)
        {
            fogList[fogWave++].SetActive(true);
            safeCollider.size = new Vector3(safeCollider.size.x - 6, 5, safeCollider.size.z - 6);

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
            else if (gameEndTime < gameNextTime)
            {
                gameNextTime -= 1;
                timeText.text = gameNextTime.ToString();
                StartCoroutine(BoomText(7));
            }
        }
    }

    public void BeTheKing(UnitInfo player)
    {
        hasKing = true;
        Debug.Log(player.name);
        player.BeTheKing();
        gameEndTime = 18;
        timeText.text = player.nickname + " is the KING";
        StartCoroutine(BoomText(5));
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

    public void DiePlayer(UnitInfo killer, UnitInfo killed)
    {
        playerCount--;
        KillLogManager.instance.AddKillData(killer, killed);
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
