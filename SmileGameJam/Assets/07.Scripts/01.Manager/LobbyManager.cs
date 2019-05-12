using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public Player nowPlayer;
    public Joystick moveJoystick;
    public Button skillButton;

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
}