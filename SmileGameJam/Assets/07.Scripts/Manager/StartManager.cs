using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour {

    public Text nicknameText;
    private float delay = 0;

    public GameObject loadingPanel;
    public Text contextText;
    public string[] contextList;

    public void Start()
    {
        nicknameText.text = PlayerPrefs.GetString("Name");
        contextText.text = contextList[Random.Range(0, contextList.Length)];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (delay > 0)
                Application.Quit();
            else
                delay = 0.15f;
        }
        if (delay > 0)
            delay -= Time.deltaTime;
    }
}
