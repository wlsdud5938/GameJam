using System.Collections;
using System;
using UnityEngine;

public class PlayerBullet : BulletBase {

    protected override void Update()
    {
        base.Update();
    }

    public override void SetInformation(Player owner, int power,float speed, float range)
    {
        base.SetInformation(owner, power, speed, range);
    }
}
