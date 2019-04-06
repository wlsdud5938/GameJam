﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfo : MonoBehaviour {

    private GameObject mainCam;
    public Transform playerCanavas;
    public Image healthBarImg;
    public Text healthText;
    public Text nicknameText;
    public Text scoreText;

    public string nickname = "Player";

    public int score = 0;
    public bool king = false;
    public float healthPoint = 100.0f;
    public float maxHealthPoint = 100.0f;
    public float rot = 0;

    public GameObject crown;

    public float defensivePower = 1.0f;
    public float heal = 0.05f;
    public float stopDist = 5.0f;

    public bool isDead = false;
    float healTime = 0.0f;
    bool hit = false;
    Animator animator;
    Rigidbody rb;
    private void Start()
    {
        if(gameObject.tag == "Enemy")
            animator = gameObject.transform.GetChild(0).GetComponent<Animator>();    
        rb = gameObject.GetComponent<Rigidbody>();
        nickname = PlayerPrefs.GetString("Name");
        nicknameText.text = nickname;
        mainCam = Camera.main.gameObject;
    }

    private void Update()
    {
        healthBarImg.fillAmount = healthPoint / maxHealthPoint;
        healthText.text = Mathf.RoundToInt(Mathf.Clamp(healthPoint,0,maxHealthPoint)).ToString();

        scoreText.text = score.ToString();

        if (healthPoint < maxHealthPoint && healTime < 0.0f)
        {
            healthPoint += maxHealthPoint * heal;
            if (maxHealthPoint < healthPoint)
                healthPoint = maxHealthPoint;
            healTime = 3.0f;
        }
        if (score >= 10 && !GameManager.Instance.king && !GameManager.Instance.imKing)
        {
            if (gameObject.tag == "Player")
                GameManager.Instance.imKing = true;
            else
                GameManager.Instance.king = true;
            king = true;
            crown.SetActive(true);

        }
        if(healthPoint <= 0)
        {
            if (!isDead && gameObject.tag == "Enemy")
            {
                isDead = true;
                Destroy();
            }

            if(gameObject.tag == "Player")
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerMove>().Death();
            }
        }
        healTime -= 1.0f * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        healthPoint = Mathf.Clamp(healthPoint - damage, 0, maxHealthPoint);
        hit = true;
        healTime = 5.0f;
    }

    private void LateUpdate()
    {
        playerCanavas.LookAt(mainCam.transform.position);
        if (crown.activeSelf)
        {
            if (rot < 360)
                rot += Time.deltaTime * 60;
            else
                rot = 0;
            crown.transform.eulerAngles = new Vector3(0, rot, 0);
        }
    }

    public void Death()
    {
        animator.SetBool("IsDead", true);
        rb.velocity = Vector3.zero;
    }

    private void Destroy()
    {

        GameManager.Instance.enemyCount--;
        Death();
        Destroy(gameObject, 2);

    }

}
