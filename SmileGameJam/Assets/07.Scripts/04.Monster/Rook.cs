using System.Collections;
using UnityEngine;

public class Rook : MonoBehaviour, IDamageable
{
    [Header("[Information]")]
    public int healthPoint = 10;
    private int maxHealthPoint = 10;

    public int basicHP = 10;
    public float stageMulti = 0.1f;

    private bool isDead = false;

    [Header("[Pattern]")]
    private float turnDuration = 0.5f;
    public float turnDelay = 0.5f;
    public LayerMask unwalkableMask;    //장애물 레이어 마스크
    public Transform monopoly;
    public Vector3 monopolyPosition;

    [Header("[Attack]")]
    public ParticleSystem bang;
    public int attack = 5;
    public int rangeDist = 3;
    public float explosionRange = 1;
    private Transform target;

    private Room parentRoom;

    private Animator animator;
    private Rigidbody rb;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        animator = transform.GetChild(0).GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        monopoly.position = monopolyPosition;
        if (parentRoom.nowTime > turnDuration)
        {
            turnDuration += turnDelay;
            AttackPattern();
        }
    }

    public void SetInfo(int nowStage, Room room)
    {
        maxHealthPoint = healthPoint = (int)(basicHP * (1 + stageMulti * (nowStage - 1)));
        parentRoom = room;
    }

    public void TakeDamage(IDamageable owner, int damage)
    {
        healthPoint = Mathf.Clamp(healthPoint - damage, 0, maxHealthPoint);

        if (healthPoint <= 0)
            Death(owner);
    }

    public void TakeHeal(int heal)
    {
        healthPoint = Mathf.Clamp(healthPoint + heal, 0, maxHealthPoint);
    }

    public void Death(IDamageable killer)
    {
        if (isDead) return;
        isDead = true;
        if (parentRoom != null)
            parentRoom.monsterCount--;
        Destroy(gameObject);
    }

    public void AttackPattern()
    {
        if (Vector3.SqrMagnitude(target.position - transform.position) <= rangeDist * rangeDist)
            StartCoroutine(AttackAni());
        else
            StartCoroutine(MoveAni(NearestDir()));
    }

    public IEnumerator AttackAni()
    {
        yield return StartCoroutine(MoveAni(new Vector2(Mathf.FloorToInt(target.position.x) + 0.5f, Mathf.FloorToInt(target.position.z) + 0.5f) - new Vector2(transform.position.x, transform.position.z)));
        bang.Play();
        foreach(Collider col in Physics.OverlapSphere(transform.position, explosionRange * 0.5f))
        {
            if (col.CompareTag("Player"))
                col.GetComponent<Player>().TakeDamage(this, attack);
        }
    }

    public IEnumerator MoveAni(Vector2 dir)
    {
        float jumpSpeed = 3;
        if (dir.x != 0 || dir.y != 0)
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
        for (int i = -rangeDist; i < rangeDist; i++)
        {
            for (int j = -rangeDist; j < rangeDist; j++)
            {
                if(Vector2.SqrMagnitude(new Vector2(i, j)) <= rangeDist * rangeDist)
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
