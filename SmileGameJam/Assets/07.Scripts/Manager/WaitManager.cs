using UnityEngine;
using UnityEngine.UI;

public class WaitManager : MonoBehaviour
{
    public GameObject NameInputFiled;
    public GameObject clickText;

    public InputField nicknameText;

    private float delay = 0;

    public SceneDirector sceneDirector;

    public void ClickToStart()
    {
        NameInputFiled.SetActive(true);
        clickText.SetActive(false);
        if (nicknameText.text == "")
            nicknameText.text = PlayerPrefs.GetString("Name", string.Empty);
    }

    public void EnterName()
    {
        PlayerPrefs.SetString("Name", nicknameText.text);
        sceneDirector.GoScene("StartScene");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NameInputFiled.SetActive(false);
            clickText.SetActive(true);
            if (delay > 0)
                Application.Quit();
            else
                delay = 0.15f;
        }
        if (delay > 0)
            delay -= Time.deltaTime;
    }
}
