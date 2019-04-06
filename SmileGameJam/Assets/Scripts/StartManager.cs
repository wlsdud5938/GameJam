using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour {

    public Text nicknameText; 

    public void Start()
    {
        nicknameText.text = PlayerPrefs.GetString("Name");
    }
}
