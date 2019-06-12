using UnityEngine;

public enum Rare { 기본, 일반, 희귀, 영웅, 전설}
public enum Shake { 끄기, 반대로, 랜덤으로 }

public abstract class GunBase : MonoBehaviour
{
    protected Rare rare;
    public Projectile nowBullet;

    public bool isBasic = false;

    public int poolSize = 100;
    [HideInInspector]
    public string id;

    [Header("[Information]")]
    protected int attack = 5;
    [HideInInspector]
    public int maxCapacity = 10, nowCapacity = 10;
    protected float bulletSpeed = 10;
    [HideInInspector]
    public float shotDelay = 0.5f;
    protected float accurancy, range = 6;

    public ParticleSystem shotParticle;

    [Header("[Shake]")]
    protected Shake shake;
    protected float shakeThrust = 0.3f;

    public virtual void Awake()
    {
        nowBullet = ObjectData.instance.bullets[id + "Bullet"];
        nowBullet.isEnemy = false;
        nowBullet.id = id + "Explosion";
        BulletPooler.instance.CreatePool(id, nowBullet, poolSize);

        nowCapacity = maxCapacity;
    }

    public void Shot(Player owner, Vector3 position, float rotation)
    {
        if (nowCapacity <= 0) return;

        if(!isBasic)
            nowCapacity--;

        UseSkill(owner, position, rotation);
        shotParticle.Play();
    }

    public void ShotFinish()
    {
        
    }

    protected abstract void UseSkill(Player owner, Vector3 position, float rotation);
}
