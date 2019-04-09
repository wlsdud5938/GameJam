using UnityEngine;

public partial class UnitInfo : MonoBehaviour {

    [Header("Poison")]
    public bool isPoisoned = false;
    private int poisonDamage;
    private float poisonTime;

    public void EnterPoison(int damage)
    {
        isPoisoned = true;
        poisonDamage = damage;
    }

    public void ExitPoison()
    {
        isPoisoned = false;
        poisonTime = 0;
    }
}
