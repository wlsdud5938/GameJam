using UnityEngine;

[CreateAssetMenu(fileName = "총기류", menuName = "총기류/라이플")]
public class RifleGunBase : ScriptableObject
{
    public Rare rare;

    public string id;
    public int poolSize = 100;

    [Header("[Information]")]
    public int attack = 5;
    public int maxCapacity = 10, nowCapacity = 10;
    public float bulletSpeed = 10;
    public float shotDelay = 0.5f;
    public float accurancy, range = 6;

    [Header("[Shake]")]
    public Shake shake;
    public float shakeThrust = 0.3f;

    [Header("[Rifle Gun]")]
    public float bulletDelay = 0.5f;
}