﻿using UnityEngine;

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
    private GameObject[] gunUI;
    public int index = 0;
    private float nowTerm = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
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
        if (isDead) return;
        GunBase newGun = Instantiate(ObjectData.instance.guns[id], transform.position, Quaternion.identity, hand);
        newGun.transform.localPosition = newGun.transform.localEulerAngles = Vector3.zero;
        gunInventory[0].gameObject.SetActive(false);

        //if (gunInventory[1] != null)
        //{
        //    Rigidbody newItem = Instantiate(ObjectData.instance.items[gunInventory[1].id.Split('(')[0] + "Item"], transform.position + Vector3.up * 0.5f, Quaternion.identity).GetComponent<Rigidbody>();
        //    newItem.AddForce(Vector3.up * 5, ForceMode.Impulse);
        //    Destroy(gunInventory[1].gameObject);
        //}
        if (gunInventory[1] != null)
            Destroy(gunInventory[1].gameObject);
        gunInventory[1] = newGun;
        nowGun = gunInventory[index = 1];

        SetGunInfo();
    }

    public void RemoveGun()
    {
        if (isDead) return;
        nowGun = gunInventory[index = 0];
        nowGun.gameObject.SetActive(true);

        Destroy(gunInventory[1].gameObject);
        gunInventory[1] = null;

        SetGunInfo();
    }

    private void SetGunInfo()
    {
        if (isDead) return;
        if (nowGun == null)
            return;
        for (int i = 0; i < gunUI.Length; i++)
        {
            if (gunUI[i].name.Substring(0, gunUI[i].name.Length - 2) == nowGun.id)
                gunUI[i].SetActive(true);
            else
                gunUI[i].SetActive(false);
        }
        nowTerm = 0;

        SetGunUI();
    }

    private void SetGunUI()
    {
        if (isDead) return;
        if (nowGun == null)
            return;

        if (nowGun.isBasic)
            bulletCountText.text = "x ∞";
        else
            bulletCountText.text = "x " + nowGun.nowCapacity + "/" + nowGun.maxCapacity;
    }

    private void ShotComplete()
    {
        animator.SetBool("IsAttacking", false);
    }

    public void AttackJoystickStay(float dist, float rotation)
    {
        if (isDead) return;
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
            //nowTerm = 0;
            animator.SetBool("IsAttacking", false);
        }
    }

    public void AttackJoystickUp(bool isMoved)
    {
        if (isDead) return;
        //nowTerm = 0;
        animator.SetBool("IsAttacking", false);
    }

    private void LateUpdate()
    {
        if (isDead) return;
        muzzle.eulerAngles = new Vector3(0, muzzleRot, 0);
    }
}