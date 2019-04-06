using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour {

    public int score = 0;

    public float healthPoint = 100.0f;
    public float maxHealthPoint = 100.0f;

    public float defensivePower = 1.0f;

    public float stopDist = 5.0f;
    private void Update()
    {
        if(score >= 10 && !GameManager.Instance.king)
        {
            GameManager.Instance.king = true;
            if (gameObject.tag == "Player")
                GameManager.Instance.imKing = true;
        }
    }


}
