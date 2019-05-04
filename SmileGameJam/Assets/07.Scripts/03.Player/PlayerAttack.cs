using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : JoystickBase
{
    [Header("Muzzle")]
    public Transform muzzle;
    private float rot = 0;
    private float muzzleRot
    {
        set
        {
            rot = value;
        }
        get
        {
            return rot + 90;
        }
    }

    [Header("Gun")]
    public int index = 0, maxCount = 2;
    public GunBase nowGun;
    public GunBase[] gunInventory;

    [Header("Gun Info")]
    public Image gunImage;
    public Text bulletCountText;
    private float nowTerm = 0, bulletTerm;

    private Animator animator;
    private Player player;

    private void Awake()
    {
        player = transform.GetComponent<Player>();
        animator = transform.GetComponent<Animator>();
        joystick = GameObject.Find("AttackJoyStick").GetComponent<RectTransform>();
        gunImage = GameObject.Find("GunImg").GetComponent<Image>();
        bulletCountText = GameObject.Find("BulletInfo").GetComponent<Text>();
    }

    protected override void Start()
    {
        base.Start();
        SetGunInfo();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SwitchGun()
    {
        index = index + 1 >= maxCount ? 0 : index + 1;
        nowGun = gunInventory[index];

        SetGunInfo();
    }

    public void GetGun(int id)
    {
        //총 잡기
        //gunInventory[1] = gunData[id];

        maxCount++;

        SetGunInfo();
    }

    public void RemoveGun()
    {
        index = 0;
        nowGun = gunInventory[index];

        Destroy(gunInventory[1].gameObject);
        gunInventory[1] = null;

        maxCount--;

        SetGunInfo();
    }


    private void SetGunInfo()
    {
        nowTerm = bulletTerm = nowGun.bulletTerm;
        //이미지 교체
        SetGunUI();
    }

    private void SetGunUI()
    {
        if (nowGun.isBasic)
            bulletCountText.text = "∞";
        else
            bulletCountText.text = nowGun.nowBulletCount + "/" + nowGun.maxBulletCount;
    }

    private void ShotComplete()
    {
        animator.SetBool("IsAttacking", false);
    }

    protected override void GetJoystickDown()
    {
        muzzleRot = rotation;
    }

    protected override void GetJoystickStay(float dist)
    {
        muzzleRot = rotation;

        if(dist >= 1)
        {
            if (nowTerm <= 0)
            {
                nowGun.Shot(player, muzzle.position, rotation);
                SetGunUI();

                if (nowGun.nowBulletCount <= 0)
                    RemoveGun();

                nowTerm = bulletTerm;
                animator.SetBool("IsAttacking", true);
            }
            else
                nowTerm -= Time.deltaTime;
        }
        else
        {
            muzzleRot = 0;
            nowTerm = bulletTerm;
            animator.SetBool("IsAttacking", false);
        }
    }

    protected override void GetJoystickUp(bool isClicked)
    {
        muzzleRot = 0;
        nowTerm = bulletTerm;
        animator.SetBool("IsAttacking", false);
    }

    private void LateUpdate()
    {
        muzzle.eulerAngles = new Vector3(0, muzzleRot, 0);
    }
}