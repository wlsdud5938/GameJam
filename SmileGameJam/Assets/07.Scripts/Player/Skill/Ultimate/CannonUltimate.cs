using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonUltimate : UltBase
{
    public int damage = 0;
    public float range = 5, speed = 10;

    public CannonBullet cannon;

    public override void UseUltimate(Vector3 position, float rotation, UnitInfo owner)
    {
        CannonBullet newCannon = Instantiate(cannon, position, Quaternion.Euler(0, rotation, 0));
        newCannon.SetInformation(damage, speed, range, owner);
    }
}
