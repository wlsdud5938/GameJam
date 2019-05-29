public interface IDamageable
{
    void TakeDamage(IDamageable attacker, int damage);
    void TakeHeal(int heal);

    void Death(IDamageable killer);
}