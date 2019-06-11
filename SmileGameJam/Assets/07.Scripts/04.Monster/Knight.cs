using System.Collections;
using UnityEngine;

public class Knight : Monster
{
    public int stopDist = 4;

    public override void AttackPattern()
    {
        StartCoroutine(AttackAni());
    }

    public override void MovePattern()
    {
        StartCoroutine(MoveAni());
    }

    public IEnumerator AttackAni()
    {
        yield return StartCoroutine(MoveAni());

        Vector3 position = transform.position + Vector3.up;
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        Projectile newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0,rotation.eulerAngles.y,0));
        newBullet.SetInformation(null, attack, bulletSpeed, 30);
        newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, rotation.eulerAngles.y + 30, 0));
        newBullet.SetInformation(null, attack, bulletSpeed, 30);
        newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, rotation.eulerAngles.y - 30, 0));
        newBullet.SetInformation(null, attack, bulletSpeed, 30);
    }

    public IEnumerator MoveAni()
    {
        yield return StartCoroutine(JumpAni(NearestDir(), 4.2f));
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(JumpAni(NearestDir(), 4.2f));
    }

    private IEnumerator JumpAni(int dir, float jumpSpeed)
    {
        if (dir != -1)
        {
            monopolyPosition = HeightZero(transform.position + Quaternion.Euler(0, 90 * dir, 0) * Vector3.forward);
            Vector3 originPos = HeightZero(transform.position);

            for (float i = 0; i < 1; i += Time.deltaTime * jumpSpeed)
            {
                transform.localScale = new Vector3(1 + i * 0.15f, 1 - i * 0.2f, 1 + i * 0.15f);
                yield return null;
            }
            transform.localScale = Vector3.one;

            for (float i = 0; i < 1; i += Time.deltaTime * jumpSpeed)
            {
                transform.position = new Vector3(originPos.x, Mathf.Sin(Mathf.PI * i) * 1.5f, originPos.z) + Quaternion.Euler(0, 90 * dir, 0) * Vector3.forward * i;
                yield return null;
            }
            transform.position = HeightZero(originPos + Quaternion.Euler(0, 90 * dir, 0) * Vector3.forward);
        }
        else
        {
            monopolyPosition = HeightZero(transform.position);
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
            transform.position = HeightZero(transform.position);
        }
    }

    private int NearestDir()
    {
        int dir = -1;
        float minDist = float.MaxValue;
        if (Vector3.SqrMagnitude(target.position - transform.position) > stopDist * stopDist)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 checkPos = transform.position + Quaternion.Euler(0, 90 * i, 0) * Vector3.forward;
                Ray ray = new Ray(checkPos + Vector3.up * 5, Vector3.down);
                if (!Physics.Raycast(ray, 100, unwalkableMask))
                {
                    float dist = Vector3.SqrMagnitude(target.position - checkPos);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        dir = i;
                    }
                }
            }
        }
        return dir;
    }
}
