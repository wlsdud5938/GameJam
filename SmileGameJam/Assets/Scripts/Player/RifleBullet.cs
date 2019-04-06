using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : BulletBase {

    protected override void Update()
    {
        base.Update();
    }

    public override void SetInformation(int power,float speed, float range)
    {
        base.SetInformation(power, speed, range);
        transform.localScale = Vector3.one * (power + 0.5f) * 1.2f;
    }
}
