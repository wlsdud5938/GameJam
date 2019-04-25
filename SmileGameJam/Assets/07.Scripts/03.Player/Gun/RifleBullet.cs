using System.Collections;
using System;
using UnityEngine;

public class RifleBullet : BulletBase {

    protected override void Update()
    {
        base.Update();
    }

    public override void SetInformation(Player owner, int power,float speed, float range)
    {
        base.SetInformation(owner, power, speed, range);
        transform.localScale = Vector3.one * (power + 0.5f) * 0.1f;
    }
}
