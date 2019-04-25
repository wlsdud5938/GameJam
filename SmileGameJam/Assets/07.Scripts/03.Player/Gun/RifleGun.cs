using System.Collections;
using System;
using UnityEngine;

public class RifleGun : GunBase
{
    protected override void UseSkill(Player owner, Vector3 position, float rotation)
    {
        BulletBase newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, rotation, 0));
        newBullet.SetInformation(owner, damage, bulletSpeed, bulletRange);
    }
}
