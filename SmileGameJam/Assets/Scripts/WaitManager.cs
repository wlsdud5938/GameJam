using UnityEngine;
using UnityEngine.UI;

public class WaitManager : MonoBehaviour {

    public GameObject NameInputFiled;
    public GameObject clickText;

    public InputField nicknameText;

    public SceneDirector sceneDirector;

    public void ClickToStart()
    {
        NameInputFiled.SetActive(true);
        clickText.SetActive(false);
        nicknameText.text = PlayerPrefs.GetString("Name", string.Empty);
    }

    public void EnterName(string name)
    {
        PlayerPrefs.SetString("Name", name);
        sceneDirector.GoScene("StartScene");
    }
}
