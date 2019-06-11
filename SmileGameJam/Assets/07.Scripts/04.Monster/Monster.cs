using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour, IDamageable {

    [Header("[Information]")]
    public int healthPoint = 10;
    private int maxHealthPoint = 10;

    public int basicHP = 10;
    public float stageMulti = 0.1f;

    private bool isDead = false;

    public string id;

    [Header("[Pattern]")]
    public int attackTurn = 3;
    private int turn = 0;
    private float turnDuration = 0.5f;
    public float turnDelay = 0.5f;
    public LayerMask unwalkableMask;    //장애물 레이어 마스크
    public Transform monopoly;
    public Vector3 monopolyPosition;

    [Header("[Attack]")]
    public Projectile nowBullet;
    protected Transform target;
    public int attack = 5;
    public int poolSize = 10;
    public float bulletSpeed = 10;

    private Room parentRoom;

    private Animator animator;
    private Rigidbody rb;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        animator = transform.GetChild(0).GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody>();

        nowBullet = ObjectData.instance.bullets[id + "Bullet"];
        nowBullet.isEnemy = true;
        nowBullet.id = id + "Explosion";
        BulletPooler.instance.CreatePool(id, nowBullet, poolSize);
    }

    protected void Update()
    {
        monopoly.position = monopolyPosition;
        if (parentRoom.nowTime > turnDuration)
        {
            turnDuration += turnDelay;
            turn++;
            if (turn % attackTurn == 0)
                AttackPattern();
            else
                MovePattern();
        }
    }

    public void SetInfo(int nowStage, Room room)
    {
        maxHealthPoint = healthPoint = (int)(basicHP * (1 + stageMulti * (nowStage - 1)));
        parentRoom = room;
    }

    public void TakeDamage(IDamageable owner, int damage)
    {
        healthPoint = Mathf.Clamp(healthPoint - damage, 0, maxHealthPoint);

        if (healthPoint <= 0)
            Death(owner);
    }

    public void TakeHeal(int heal)
    {
        healthPoint = Mathf.Clamp(healthPoint + heal, 0, maxHealthPoint);
    }

    public void Death(IDamageable killer)
    {
        if (isDead) return;
        isDead = true;
        if (parentRoom != null)
            parentRoom.monsterCount--;
        Destroy(gameObject);
    }

    public abstract void MovePattern();
    public abstract void AttackPattern();

    public Vector3 HeightZero(Vector3 v)
    {
        return new Vector3(v.x, 0, v.z);
    }
}
