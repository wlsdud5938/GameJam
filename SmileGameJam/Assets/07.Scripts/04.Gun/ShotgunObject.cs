using UnityEngine;

[CreateAssetMenu(menuName = "총기류/샷건")]
public class ShotgunObject : ScriptableObject
{
    public string name = "샷건";

    public Rare rare;

    public string id;
    public int poolSize = 100;

    [Header("[Information]")]
    public int attack = 5;
    public int maxCapacity = 10, nowCapacity = 10;
    public float bulletSpeed = 10;
    public float shotDelay = 0.5f;
    public float accurancy, range = 6;
    public bool canPenetrate = false;

    [Header("[Shake]")]
    public Shake shake;
    public float shakeThrust = 0.3f;

    [Header("[Shot Gun]")]
    public float angle = 60;
    public int bulletCount = 10;
}