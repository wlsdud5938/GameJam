using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
    void TakeDamage(IDamageable attacker, int damage);
    void TakeHeal(int heal);

    void Death(IDamageable killer);
}
