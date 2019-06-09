using System.Collections;
using UnityEngine;

public class Pawn : Monster
{
    public override void AttackPattern()
    {
        StartCoroutine(AttackAni());
    }

    public override void MovePattern()
    {
        StartCoroutine(MoveAni(Vector3.zero));
    }

    public IEnumerator AttackAni()
    {
        yield return StartCoroutine(MoveAni(Vector3.zero));

        Vector3 position = transform.position + Vector3.up;
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        Projectile newBullet = BulletPooler.instance.ReuseObject(id, position, rotation);
        newBullet.SetInformation(null, attack, bulletSpeed, 30);
    }

    public IEnumerator MoveAni(Vector3 direction)
    {
        int dir = -1;
        float minDist = float.MaxValue;
        for(int i = 0; i < 4; i++)
        {
            Ray ray = new Ray(transform.position + Vector3.up * 5, Vector3.down);
            if (!Physics.Raycast(ray, 100, unwalkableMask))
            {
                float dist = Vector3.Distance();
            }
        }

        float jumpSpeed = 3.0f;
        Vector3 originPos = transform.position;

        for (float i = 0; i < 1; i += Time.deltaTime * jumpSpeed)
        {
            transform.localScale = new Vector3(1 + i * 0.15f, 1 - i * 0.2f, 1 + i * 0.15f);
            yield return null;
        }
        transform.localScale = Vector3.one;

        for (float i = 0; i < 1; i += Time.deltaTime * jumpSpeed)
        {
            transform.position = new Vector3(originPos.x, Mathf.Sin(Mathf.PI * i) * 1.5f, originPos.z) + direction * i;
            yield return null;
        }
    }
}
