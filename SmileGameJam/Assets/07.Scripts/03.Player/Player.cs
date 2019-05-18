using UnityEngine;
using UnityEngine.UI;

public partial class Player : MonoBehaviour {

    [Header("Information")]
    public GunBase nowGun;
    public int coin = 0;
    public float moveSpeed = 3.5f;
    private int maxHealthPoint, healthPoint;

    public bool canAttack = true;

    private GameObject[] hearts;
    private Text coinText;
    private Image gunImage;
    private Text bulletCountText;

    public float invincibilityDelay = 0.2f;
    private float invincibilityTime;

    private Transform playerTr;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        playerTr = transform;
        muzzle = transform.GetChild(1);
        rb = transform.GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        coinText = GameObject.Find("CoinText").GetComponent<Text>();
        gunImage = GameObject.Find("GunImg").GetComponent<Image>();
        bulletCountText = GameObject.Find("BulletInfo").GetComponent<Text>();

        if (canAttack)
        {
            Transform healthTr = GameObject.Find("Health").transform;
            hearts = new GameObject[healthTr.childCount];
            for (int i = 0; i < healthTr.childCount; i++)
                hearts[i] = healthTr.GetChild(i).gameObject;
            maxHealthPoint = healthPoint = healthTr.childCount;

            nowGun = gunInventory[0];
        }
    }

    private void Start()
    {
        SetInfo();
        SetGunInfo();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) TakeDamage(null, 1);
        if (Input.GetKeyDown(KeyCode.G)) TakeHeal(1);
        if (Input.GetKeyDown(KeyCode.Space)) Roll();


        if (isRolling)
        {
            RollAnim();
        }
        else
        {
            playerTr.rotation = Quaternion.Slerp(playerTr.rotation, Quaternion.Euler(0, targetRot, 0), Time.deltaTime * rotSpeed);
            if (!isMoving)
                rb.velocity = Vector3.zero;
        }
        if (invincibilityTime > 0)
            invincibilityTime -= Time.deltaTime;

        if (nowTerm > 0)
            nowTerm -= Time.deltaTime;
    }

    private void SetInfo()
    {
        for(int i = 0; i < maxHealthPoint; i++)
        {
            if (i < healthPoint)
                hearts[i].SetActive(true);
            else
                hearts[i].SetActive(false);
        }
        coinText.text = coin.ToString();
    }

    public void TakeDamage(Monster owner, int damage)
    {
        if (isRolling || invincibilityTime > 0) return;

        invincibilityTime = invincibilityDelay;
        healthPoint = Mathf.Clamp(healthPoint - damage, 0, maxHealthPoint);

        if (healthPoint <= 0)
        {
            Debug.Log("Death");
        }
        SetInfo();
    }

    public void TakeHeal(int heal)
    {
        healthPoint = Mathf.Clamp(healthPoint + heal, 0, maxHealthPoint);
        SetInfo();
    }

    public void GetCoin(int coin)
    {
        this.coin += coin;
        SetInfo();
    }
}
