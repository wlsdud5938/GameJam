using UnityEngine;
using System.Collections.Generic;

public partial class Player : MonoBehaviour
{
    [Header("Muzzle")]
    public Transform muzzle;
    public Transform hand;
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
    public GunBase nowGun;
    public GunBase[] gunInventory;
    public int index = 0;
    private float nowTerm = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            GetGun(other.GetComponent<ItemCtrl>().id);
            Destroy(other.gameObject);
        }
    }

    public void GetGun(string id)
    {
        nowGun.gameObject.SetActive(false);
        if (gunInventory[1] == null)
        {
            GunBase newGun = Instantiate(ObjectData.instance.guns[id], Vector3.zero, Quaternion.identity, hand);
            newGun.transform.localPosition = newGun.transform.localEulerAngles = Vector3.zero;
            gunInventory[1] = newGun;
            nowGun = gunInventory[index = 1];
            nowGun.Start();
        }
        else
        {
            Destroy(gunInventory[1].gameObject);
            ItemCtrl newItem = Instantiate(ObjectData.instance.items[id], transform.position, Quaternion.identity, null);

            GunBase newGun = Instantiate(ObjectData.instance.guns[id], Vector3.zero, Quaternion.identity, hand);
            newGun.transform.localPosition = newGun.transform.localEulerAngles = Vector3.zero;
            gunInventory[1] = newGun;
            nowGun = gunInventory[index = 1];
            nowGun.Start();
        }

        SetGunInfo();
    }

    public void RemoveGun()
    {
        nowGun = gunInventory[index = 0];
        nowGun.gameObject.SetActive(true);

        Destroy(gunInventory[1].gameObject);
        gunInventory[1] = null;

        SetGunInfo();
    }

    private void SetGunInfo()
    {
        if (nowGun == null)
            return;
        nowTerm = 0;

        foreach(KeyValuePair<string, GameObject> gun in gunImage)
        {
            Debug.Log(nowGun.id + "UI" + " : " + gun.Key);
            if(gun.Key.Equals(nowGun.id + "UI"))
                gun.Value.SetActive(true);
            else
                gun.Value.SetActive(false);
        }
        SetGunUI();
    }

    private void SetGunUI()
    {
        if (nowGun == null)
            return;

        if (nowGun.isBasic)
            bulletCountText.text = "∞";
        else
            bulletCountText.text = nowGun.Capacity;
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