using UnityEngine;

public class DestroyBox : MonoBehaviour
{
    public GameObject brokenBox;
    bool isBroken = false;

    public void TakeDamage(int damage)
    {
        if (isBroken) return;

        isBroken = true;
        GameObject obj = Instantiate(brokenBox, gameObject.transform.position, Quaternion.identity);
        Destroy(obj, 2.0f);
        Destroy(gameObject);
    }
}
