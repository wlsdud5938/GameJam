using System.Collections;
using UnityEngine;

public class Rook : Monster
{
    public int rangeDist = 3;
    public int explosionRange = 1;
    public ParticleSystem bang;

    public override void AttackPattern()
    {
        if (Vector3.SqrMagnitude(target.position - transform.position) < rangeDist * rangeDist)
            StartCoroutine(AttackAni());
        else
            StartCoroutine(MoveAni(NearestDir()));
    }

    public override void MovePattern()
    {
        AttackPattern();
    }

    public IEnumerator AttackAni()
    {
        yield return StartCoroutine(MoveAni(new Vector2(Mathf.RoundToInt(target.position.x), Mathf.RoundToInt(target.position.z))));
        bang.Play();
        foreach(Collider col in Physics.OverlapSphere(transform.position, explosionRange * 0.5f))
        {
            if (col.CompareTag("Player"))
                col.GetComponent<Player>().TakeDamage(this, attack);
        }
    }

    public IEnumerator MoveAni(Vector2 dir)
    {
        monopoly.position = transform.position + new Vector3(dir.x, 0, dir.y);
        float jumpSpeed = 3;
        if (dir.x != 0 || dir.y != 0)
        {
            Vector3 originPos = transform.position;

            for (float i = 0; i < 1; i += Time.deltaTime * jumpSpeed)
            {
                transform.localScale = new Vector3(1 + i * 0.15f, 1 - i * 0.2f, 1 + i * 0.15f);
                yield return null;
            }
            transform.localScale = Vector3.one;

            for (float i = 0; i < 1; i += Time.deltaTime * jumpSpeed)
            {
                transform.position = new Vector3(originPos.x, Mathf.Sin(Mathf.PI * i) * 1.5f, originPos.z) + new Vector3(dir.x, 0, dir.y) * i;
                yield return null;
            }
            transform.position = originPos + new Vector3(dir.x, 0, dir.y);
        }
        else
        {
            for (float i = 0; i < 1; i += Time.deltaTime * jumpSpeed)
            {
                transform.localScale = new Vector3(1 + i * 0.15f, 1 - i * 0.2f, 1 + i * 0.15f);
                yield return null;
            }
            transform.localScale = Vector3.one;

            for (float i = 0; i < 1; i += Time.deltaTime * jumpSpeed)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Sin(Mathf.PI * i) * 1.5f, transform.position.z);
                yield return null;
            }
        }
    }

    private Vector2 NearestDir()
    {
        int x = 0, y = 0;
        float minDist = float.MaxValue;
        for (int i = -rangeDist; i < rangeDist; i++)
        {
            for (int j = -rangeDist; j < rangeDist; j++)
            {
                if(Vector2.SqrMagnitude(new Vector2(i, j)) <= rangeDist)
                {
                    Vector3 checkPos = transform.position + new Vector3(i, 0, j);
                    Ray ray = new Ray(checkPos + Vector3.up * 5, Vector3.down);
                    if (!Physics.Raycast(ray, 100, unwalkableMask))
                    {
                        float dist = Vector3.SqrMagnitude(target.position - checkPos);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            x = i;
                            y = j;
                        }
                    }
                }
            }
        }
        return new Vector2(x, y);
    }
}
