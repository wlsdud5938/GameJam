using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfo : MonoBehaviour {

    private GameObject mainCam;

    [Header("UI")]
    public Transform playerCanavas;
    public Image healthBarImg;
    public Text healthText;
    public Text nicknameText;
    public Text scoreText;

    [Header("Information")]
    public string nickname = "Player";
    public float healthPoint = 100.0f;
    public float maxHealthPoint = 100.0f;
    public int score = 0;
    public float stopDist = 5.0f;
    private bool isDead = false;

    [Header("Heal")]
    public float healPercent = 0.05f;
    private int healValue;
    private float healTime = 0.0f;


    [Header("King")]
    public GameObject crown;
    private bool isKing = false;
    private float rot = 0;

    private Animator animator;
    private Rigidbody rb;

    private void Start()
    {
        mainCam = Camera.main.gameObject;

        animator = transform.GetChild(0).GetComponent<Animator>();    
        rb = transform.GetComponent<Rigidbody>();

        healValue = Mathf.RoundToInt(maxHealthPoint * healPercent);
        nickname = PlayerPrefs.GetString("Name");
        nicknameText.text = nickname;
    }

    private void Update()
    {
        if(healTime > 0)
            healTime -= Time.deltaTime;
        else
        {
            TakeHeal(healValue);
            healTime = 3.0f;
        }

        if (score >= 10 && !GameManager.Instance.king && !GameManager.Instance.imKing)
        {
            if (gameObject.tag == "Player")
                GameManager.Instance.imKing = true;
            else
                GameManager.Instance.king = true;
            BeTheKing();
        }
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

    private void BeTheKing()
    {
        isKing = true;
        crown.SetActive(true);
    }

    private void SetInfo()
    {
        healthBarImg.fillAmount = healthPoint / maxHealthPoint;
        healthText.text = Mathf.RoundToInt(Mathf.Clamp(healthPoint, 0, maxHealthPoint)).ToString();

        scoreText.text = score.ToString();
    }

    public void TakeDamage(int damage)
    {
        healthPoint = Mathf.Clamp(healthPoint - damage, 0, maxHealthPoint);
        healTime = 5.0f;

        if (healthPoint <= 0)
        {
            if (!isDead && gameObject.tag == "Enemy")
            {
                isDead = true;
                Death();
            }

            if (gameObject.tag == "Player")
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerMove>().Death();
            }
        }
        SetInfo();
    }

    public void TakeHeal(int heal)
    {
        healthPoint = Mathf.Clamp(healthPoint + heal, 0, maxHealthPoint);
        SetInfo();
    }

    public void Death()
    {
        GameManager.Instance.enemyCount--;
        animator.SetBool("IsDead", true);
        rb.velocity = Vector3.zero;
        Destroy(gameObject, 2);
    }
}
