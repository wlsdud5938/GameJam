using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    [Header("Information")]
    public int maxHealthPoint = 100;
    public int healthPoint = 100;

    private Animator animator;
    private Rigidbody rb;

    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody>();
    }

    public void TakeDamage(Player owner, int damage)
    {
        healthPoint = Mathf.Clamp(healthPoint - damage, 0, maxHealthPoint);

        if (healthPoint <= 0)
        {
            Debug.Log("Death");
        }
    }

    public void TakeHeal(int heal)
    {
        healthPoint = Mathf.Clamp(healthPoint + heal, 0, maxHealthPoint);
    }
}
