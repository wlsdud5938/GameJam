using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public void TouchtoStart()
    {
        SceneManager.LoadScene(1);
    }
}