using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float playTime = 0.0f;
    public bool king = false;
    public bool imKing = false;
    public float gameEndTime = 15.0f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if(king)
        {
            gameEndTime -= 1.0f * Time.deltaTime;
        }
        if (gameEndTime < 0)
        {
            if (imKing)
            { }
        }

	}

}
