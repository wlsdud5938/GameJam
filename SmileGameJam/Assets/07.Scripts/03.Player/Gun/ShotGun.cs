using UnityEngine;

public class ShotGun : GunBase {

    public float angle = 60;
    public int bulletCount = 10;

    protected override void UseSkill(Player owner, Vector3 position, float rotation)
    {
        float startAngle = rotation - angle * 0.5f;
        float angleGap = angle / bulletCount;

        CameraManager.instance.ReboundCamera(0.15f, Quaternion.Euler(0, rotation, 0) * Vector3.forward);

        for (int i = 0; i < bulletCount; i++)
        {
            BulletBase newBullet =
                BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, startAngle + angleGap * i + UnityEngine.Random.Range(-0.1f, 0.1f), 0));

            float speed = bulletSpeed + Random.Range(-1.5f, 1.5f);
            float range = bulletRange + Random.Range(-0.3f, 0.2f);
            newBullet.SetInformation(owner, damage, speed, range);
        }
    }
}
