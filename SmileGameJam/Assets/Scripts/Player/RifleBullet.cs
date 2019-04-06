using System.Collections;
using System;
using UnityEngine;

public class RifleBullet : BulletBase {

    protected override void Update()
    {
        base.Update();
    }

    public override void SetInformation(int power,float speed, float range, PlayerAttack owner)
    {
        base.SetInformation(power, speed, range, owner);
        transform.localScale = Vector3.one * (power + 0.5f) * 1.2f;
    }
}
