using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "총기류", menuName = "총기류/라이플")]
public class ShotgunObject : ScriptableObject
{
    public string name = "라이플";

    public Rare rare;

    public string id;

    [Header("[Information]")]
    public int attack = 5;
    public int maxCapacity = 10;
    public float bulletSpeed = 10;
    public float shotDelay = 0.5f;
    public float accurancy, range = 6;
    public bool canPenetrate = false;

    [Header("[Shake]")]
    public Shake shake;
    public float shakeThrust = 0.3f;

    [Header("[Rifle Gun]")]
    public float angle = 60;
    public int bulletCount = 1;
}