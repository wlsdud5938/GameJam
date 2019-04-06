using System.Collections;
using System;
using UnityEngine;

public class ShotgunBullet : BulletBase {

    protected override void Update()
    {
        base.Update();
    }

    public override void SetInformation(int damage, float speed, float range, PlayerAttack owner)
    {
        base.SetInformation(damage, speed, range, owner);
    }
}
