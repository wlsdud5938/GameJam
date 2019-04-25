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
    public int index = 0;
    public GunBase nowGun;
    public GunBase[] gunInventory;

    private Animator animator;
    private Player player;

    private void Start()
    {
        player = transform.GetComponent<Player>();
        animator = transform.GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SwitchGun()
    {
        index = index + 1 > gunInventory.Length ? 0 : index + 1;
        nowGun = gunInventory[index];
    }

    protected override void GetJoystickDown()
    {
        muzzleRot = rotation;
    }

    protected override void GetJoystickStay(float dist)
    {
        muzzleRot = rotation;
        nowGun.GetGunStay(player, muzzle.position, muzzleRot - 90);
    }

    protected override void GetJoystickUp(bool isClicked)
    {
        muzzleRot = 0;
        nowGun.GetGunUp();
    }

    private void LateUpdate()
    {
        muzzle.eulerAngles = new Vector3(0, muzzleRot, 0);
    }
}