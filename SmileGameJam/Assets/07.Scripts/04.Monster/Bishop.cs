using System.Collections;
using UnityEngine;

public class Bishop : Monster
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
        Projectile newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0,0,0));
        newBullet.SetInformation(null, attack, bulletSpeed, 30);
        newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, 90, 0));
        newBullet.SetInformation(null, attack, bulletSpeed, 30);
        newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, 180, 0));
        newBullet.SetInformation(null, attack, bulletSpeed, 30);
        newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, 270, 0));
        newBullet.SetInformation(null, attack, bulletSpeed, 30);

        yield return new WaitForSeconds(0.3f);

        newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, 45, 0));
        newBullet.SetInformation(null, attack, bulletSpeed, 30);
        newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, 135, 0));
        newBullet.SetInformation(null, attack, bulletSpeed, 30);
        newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, 225, 0));
        newBullet.SetInformation(null, attack, bulletSpeed, 30);
        newBullet = BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, 315, 0));
        newBullet.SetInformation(null, attack, bulletSpeed, 30);
    }

    public IEnumerator MoveAni()
    {
        Vector2 dir = NearestDir();
        float jumpSpeed = 3;
        if (dir.x !=0|| dir.y != 0)
        {
            monopolyPosition = transform.position + new Vector3(dir.x, 0, dir.y);
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
            monopolyPosition = transform.position;
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
        if (Vector3.SqrMagnitude(target.position - transform.position) > stopDist * stopDist)
        {
            int tempX = 1, tempY = 1;
            Vector3 checkPos = transform.position + new Vector3(tempX, 0, tempY);
            Ray ray = new Ray(checkPos + Vector3.up * 5, Vector3.down);
            if (!Physics.Raycast(ray, 100, unwalkableMask))
            {
                float dist = Vector3.SqrMagnitude(target.position - checkPos);
                if (dist < minDist)
                {
                    minDist = dist;
                    x = tempX;
                    y = tempY;
                }
            }

            tempX = 1; tempY = -1;
            checkPos = transform.position + new Vector3(tempX, 0, tempY);
            ray = new Ray(checkPos + Vector3.up * 5, Vector3.down);
            if (!Physics.Raycast(ray, 100, unwalkableMask))
            {
                float dist = Vector3.SqrMagnitude(target.position - checkPos);
                if (dist < minDist)
                {
                    minDist = dist;
                    x = tempX;
                    y = tempY;
                }
            }

            tempX = -1; tempY = 1;
            checkPos = transform.position + new Vector3(tempX, 0, tempY);
            ray = new Ray(checkPos + Vector3.up * 5, Vector3.down);
            if (!Physics.Raycast(ray, 100, unwalkableMask))
            {
                float dist = Vector3.SqrMagnitude(target.position - checkPos);
                if (dist < minDist)
                {
                    minDist = dist;
                    x = tempX;
                    y = tempY;
                }
            }

            tempX = -1; tempY = -1;
            checkPos = transform.position + new Vector3(tempX, 0, tempY);
            ray = new Ray(checkPos + Vector3.up * 5, Vector3.down);
            if (!Physics.Raycast(ray, 100, unwalkableMask))
            {
                float dist = Vector3.SqrMagnitude(target.position - checkPos);
                if (dist < minDist)
                {
                    minDist = dist;
                    x = tempX;
                    y = tempY;
                }
            }
        }
        return new Vector2(x, y);
    }
}
