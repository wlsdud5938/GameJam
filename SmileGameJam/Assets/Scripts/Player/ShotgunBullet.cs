using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : BulletBase {

    protected override void Update()
    {
        base.Update();
    }

    public override void SetInformation(int damage, float speed, float range)
    {
        base.SetInformation(damage, speed, range);
    }
}
