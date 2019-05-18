using UnityEngine;

public partial class Player : MonoBehaviour
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
    public GunBase[] gunInventory;
    public int index = 0, maxCount = 2;
    private float nowTerm = 0;

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
        if (nowGun == null)
            return;
        nowTerm = 0;
        //이미지 교체
        SetGunUI();
    }

    private void SetGunUI()
    {
        if (nowGun == null)
            return;

        if (nowGun.isBasic)
            bulletCountText.text = "∞";
        else
            bulletCountText.text = nowGun.nowCapacity + "/" + nowGun.maxCapacity;
    }

    private void ShotComplete()
    {
        animator.SetBool("IsAttacking", false);
    }

    public void AttackJoystickStay(float dist, float rotation)
    {
        muzzleRot = rotation;
        if (nowGun == null)
            return;

        if(dist >= 1)
        {
            if (nowTerm <= 0)
            {
                nowGun.Shot(this, muzzle.position, rotation);
                SetGunUI();

                if (nowGun.nowCapacity <= 0)
                    RemoveGun();

                nowTerm = nowGun.shotDelay;
                animator.SetBool("IsAttacking", true);
            }
        }
        else
        {
            muzzleRot = 0;
            //nowTerm = 0;
            animator.SetBool("IsAttacking", false);
        }
    }

    public void AttackJoystickUp(bool isMoved)
    {
        muzzleRot = 0;
        //nowTerm = 0;
        animator.SetBool("IsAttacking", false);
    }

    private void LateUpdate()
    {
        muzzle.eulerAngles = new Vector3(0, muzzleRot, 0);
    }
}