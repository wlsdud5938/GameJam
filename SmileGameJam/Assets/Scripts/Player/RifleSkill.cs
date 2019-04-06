using System.Collections;
using System;
using UnityEngine;

public class RifleSkill : SkillBase
{
    public override void ShowRange(int power, Vector3 position, float rotation)
    {

    }

    public override void HideRange()
    {

    }

    public override void UseSkill(int power, float range, Vector3 position, float rotation, Action hitCall)
    {
        BulletBase newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, rotation, 0));
        newBullet.SetInformation(power, bulletSpeed, range, hitCall);
    }
}
