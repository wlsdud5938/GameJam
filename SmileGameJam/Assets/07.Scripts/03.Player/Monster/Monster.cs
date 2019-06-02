using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    [Header("Information")]
    private int maxHealthPoint = 100;
    public int healthPoint = 100;

    public int basicHP = 100;
    public float stageMulti = 0.1f;

    private Room parentRoom;

    private Animator animator;
    private Rigidbody rb;

    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody>();
    }

    public void SetInfo(int nowStage, Room room)
    {
        maxHealthPoint = healthPoint = (int)(basicHP * (1 + stageMulti * (nowStage - 1)));
        parentRoom = room;
    }

    public void TakeDamage(IDamageable attacker, int damage)
    {
        healthPoint = Mathf.Clamp(healthPoint - damage, 0, maxHealthPoint);

        if (healthPoint <= 0)
        {
            Death(attacker);
        }
    }

    public void TakeHeal(int heal)
    {
        healthPoint = Mathf.Clamp(healthPoint + heal, 0, maxHealthPoint);
    }

    public void Death(IDamageable killer)
    {
        if (parentRoom != null)
            parentRoom.monsterCount--;
        Destroy(gameObject);
    }
}
