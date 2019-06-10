using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public Player nowPlayer;
    public Joystick moveJoystick;
    public Button skillButton;

    private bool isPaused;
    public GameObject pausePanel;

    public CameraManager cameraManager;

    public void StartToGame()
    {
        SceneManager.LoadScene(2);
    }

    private bool flag = false;
    public void SelectPlayer(Player player)
    {
        if (nowPlayer != null)
        {
            nowPlayer.GetComponent<Rigidbody>().isKinematic = true;
            nowPlayer.enabled = false;
        }

        nowPlayer = player;
        moveJoystick.JoystickActive(false);

        if (nowPlayer != null)
        {
            flag = true;
            nowPlayer.enabled = true;
            nowPlayer.GetComponent<Rigidbody>().isKinematic = false;

            moveJoystick.GetJoystickStay.RemoveAllListeners();
            moveJoystick.GetJoystickUp.RemoveAllListeners();
            skillButton.onClick.RemoveAllListeners();
            moveJoystick.GetJoystickStay.AddListener(nowPlayer.MoveJoystickStay);
            moveJoystick.GetJoystickUp.AddListener(nowPlayer.MoveJoystickUp);
            skillButton.onClick.AddListener(nowPlayer.Roll);
        }

        cameraManager.target = nowPlayer.transform;
    }

    public void PasueButton()
    {
        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);
        if (isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Player"))
                    SelectPlayer(hit.transform.GetComponent<Player>());
            }
        }
    }

    private void LateUpdate()
    {
        if (flag)
            moveJoystick.JoystickActive(true);
        flag = false;
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