using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDirector : MonoBehaviour {

    public GameObject startScene;
    public GameObject IngameScene;

    public void GoMainScene()
    {
        startScene.SetActive(false);
        IngameScene.SetActive(true);
    }
    
    public void GoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
