using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : JoystickBase
{
    public Transform muzzle;

    [Header("Gun")]
    public GunBase nowGun;
    public int index = 0;
    public GunBase[] gunInventory;

    private Animator animator;
    private Player player;

    private void Start()
    {
        player = transform.GetComponent<Player>();
        animator = transform.GetComponent<Animator>();
    }

    public void SwitchGun()
    {
        index = index + 1 > gunInventory.Length ? 0 : index + 1;
        nowGun = gunInventory[index];
    }

    protected override void GetJoystickDown()
    {

    }

    protected override void GetJoystickStay(float dist)
    {
        nowGun.GetGunStay(player, muzzle.position, muzzle.eulerAngles.y);
    }

    protected override void GetJoystickUp(bool isClicked)
    {
        nowGun.GetGunUp();
    }
}