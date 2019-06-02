using UnityEngine;

public class ShotGun : GunBase {

    private float angle = 60;
<<<<<<< HEAD:SmileGameJam/Assets/07.Scripts/03.Player/Gun/ShotGun.cs
    public int bulletCount = 10;
=======
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
>>>>>>> parent of bf49245... .:SmileGameJam/Assets/07.Scripts/04.Gun/ShotGunBase.cs

    protected override void UseSkill(Player owner, Vector3 position, float rotation)
    {
        angle *= accurancy;
        float startAngle = rotation - angle * 0.5f;
        float angleGap = angle / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            BulletBase newBullet =
                BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, startAngle + angleGap * i + UnityEngine.Random.Range(-0.1f, 0.1f), 0));

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
