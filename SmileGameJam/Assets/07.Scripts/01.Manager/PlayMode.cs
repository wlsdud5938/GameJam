using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMode : MonoBehaviour {

    private bool isPaused = false;
    public GameObject pausePanel;

    public void PasueButton()
    {
        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);
        if (isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void StopButton()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

    public void HomeButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void SettingButton()
    {

    }

    public void PlayButton()
    {
        isPaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnApplicationQuit()
    {
        Time.timeScale = 1;
    }
}
