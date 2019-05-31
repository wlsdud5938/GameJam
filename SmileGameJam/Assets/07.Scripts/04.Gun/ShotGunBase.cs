using UnityEngine;

public class ShotGunBase : GunBase {

    public ShotgunObject data;

    private float angle = 60;
    private int bulletCount = 10;

    public override void Start()
    {
        rare = data.rare;

        id = data.id;

        attack = data.attack;
        maxCapacity = data.maxCapacity;
        bulletSpeed = data.bulletSpeed;
        shotDelay = data.shotDelay;
        accurancy = data.accurancy;
        range = data.range;

        shake = data.shake;
        shakeThrust = data.shakeThrust;

        angle = data.angle;
        bulletCount = data.bulletCount;

        base.Start();
    }

    protected override void UseSkill(Player owner, Vector3 position, float rotation)
    {
        angle *= accurancy;
        float startAngle = rotation - angle * 0.5f;
        float angleGap = angle / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float rot = startAngle + angleGap * i + Random.Range(-0.1f, 0.1f);
            Projectile newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, rot, 0));

            float speed = bulletSpeed + Random.Range(-1.5f, 1.5f);
            float range = this.range + Random.Range(-0.3f, 0.2f);
            newBullet.SetInformation(owner, attack, speed, range);
        }

        switch (shake)
        {
            case Shake.반대로:
                CameraManager.instance.ReboundCamera(0.15f, Quaternion.Euler(0, rotation, 0) * Vector3.forward);
                break;
            case Shake.랜덤으로:
                CameraManager.instance.ReboundCamera(0.15f, Quaternion.Euler(0, Random.Range(0,360), 0) * Vector3.forward);
                break;
        }
    }
}
