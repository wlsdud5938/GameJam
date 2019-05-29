using System.Collections;
using UnityEngine;

public class RifleGun : GunBase
{
    [Header("[Bullet Information]")]
    public int bulletCount = 1;
    public float bulletDelay = 0.5f;

    protected override void UseSkill(Player owner, Vector3 position, float rotation)
    {
        if (bulletCount > 1)
            StartCoroutine(ShotAnim(owner, position, rotation));
        else
        {
            rotation += Random.Range(-accurancy, accurancy);
            Projectile newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, rotation, 0));
            newBullet.SetInformation(owner, attack, bulletSpeed, range);
        }
    }

    IEnumerator ShotAnim(Player owner, Vector3 position, float rotation)
    {
        Projectile newBullet;
        for (int i = 0; i < bulletCount - 1; i++)
        {
            rotation += Random.Range(-accurancy, accurancy);
            newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, rotation, 0));
            newBullet.SetInformation(owner, attack, bulletSpeed, range);
            if (!isBasic)  nowCapacity--;
            yield return new WaitForSeconds(bulletDelay);
        }
        rotation += Random.Range(-accurancy, accurancy);
        newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, rotation, 0));
        newBullet.SetInformation(owner, attack, bulletSpeed, range);
        ShotFinish();
    }
}
