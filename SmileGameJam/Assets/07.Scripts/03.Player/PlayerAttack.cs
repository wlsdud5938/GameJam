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
    public GunBase[] gunInventory;
    public int index = 0;
    private float nowTerm = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            string itemId = other.GetComponent<ItemCtrl>().id;
            if (itemId.EndsWith("Gun"))
                GetGun(itemId);
            Destroy(other.gameObject);
        }
    }

    public void GetGun(string id)
    {
        GunBase newGun = Instantiate(ObjectData.instance.guns[id], transform.position, Quaternion.identity, hand);
        newGun.transform.localPosition = newGun.transform.localEulerAngles = Vector3.zero;
        gunInventory[0].gameObject.SetActive(false);

        if (gunInventory[1] != null)
        {
            Rigidbody newItem = Instantiate(ObjectData.instance.items[gunInventory[1].id.Split('(')[0] + "Item"], transform.position + Vector3.up * 0.5f, Quaternion.identity).GetComponent<Rigidbody>();
            newItem.AddForce(Vector3.up * 5, ForceMode.Impulse);
            Destroy(gunInventory[1].gameObject);
        }
        gunInventory[1] = newGun;
        nowGun = gunInventory[index = 1];

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