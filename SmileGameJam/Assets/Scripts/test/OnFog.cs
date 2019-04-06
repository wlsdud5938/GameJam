using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFog : MonoBehaviour {
    public float playTime = 0.0f;
    public float waveTime = 5.0f;
    public float waveStartTime = 30.0f;
    public float secondStageTime = 60.0f;
    int numOfWave;
    int i;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        playTime += 1.0f * Time.deltaTime;
        numOfWave = (int)((playTime-waveStartTime) / waveTime);
        if (playTime > waveStartTime && numOfWave != 0 && numOfWave <= 4)
        {
            for(i=0;i<4;i++)
                gameObject.transform.GetChild(i).gameObject.transform.GetChild(numOfWave - 1).gameObject.SetActive(true);
        }

	}
}

