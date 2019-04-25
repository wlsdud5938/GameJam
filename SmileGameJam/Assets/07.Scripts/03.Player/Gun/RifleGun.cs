using System.Collections;
using UnityEngine;

public class RifleGun : GunBase
{
    private float shotSpread = 0.3f;

    protected override void UseSkill(Player owner, Vector3 position, float rotation)
    {
        BulletBase newBullet = 
            BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, rotation + Random.Range(-shotSpread, shotSpread), 0));
        newBullet.SetInformation(owner, damage, bulletSpeed, bulletRange);
    }
}
