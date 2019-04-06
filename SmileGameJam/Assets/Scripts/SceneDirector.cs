using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDirector : MonoBehaviour {
    
    public void GoScene(string sceneName)
    {
        Debug.Log("Go : " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
