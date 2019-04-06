using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMoving : MonoBehaviour
{
    public enum State { Wander, Chase, Run }
    public State nowState;

    public Vector3 sight;

    public float speed = 5.0f;
    private float playerGap;
    public float delay = 0;
    public bool isWait = false;
    NavMeshAgent agent;

    public GameObject bullet;

    public Transform target = null;
    public Transform player;

    public Transform[] wayPoints;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        playerGap = GetComponent<UnitInfo>().stopDist;

        target = wayPoints[Random.Range(0, wayPoints.Length - 1)];

        StartCoroutine("ResetDestination", 0.03f);
    }

    private void ReChase()
    {
        isWait = false;
    }

    void Update()
    {
        float minDistSqr = 0;
        Collider nearestTarget = null;
        bool flag = false;

        if (isWait)
        {
            if (Vector3.SqrMagnitude(transform.position - target.position) < 0.2f)
                target = wayPoints[Random.Range(0, wayPoints.Length - 1)];
            agent.stoppingDistance = 0;
            return;
        }

        Collider[] cols = Physics.OverlapBox(transform.position + Vector3.up * sight.y * 0.5f, sight);
        foreach(Collider col in cols)
        {
            if (col.CompareTag("Player"))
            {
                if (Random.Range(0, 100) < 20)
                {
                    nowState = State.Chase;
                    target = col.transform;
                    agent.stoppingDistance = playerGap;
                    flag = true;
                    break;
                }
            }
            else if (col.CompareTag("Item"))
            {
                if (nearestTarget == null)
                {
                    nearestTarget = col;
                    minDistSqr = Vector3.SqrMagnitude(transform.position - col.transform.position);
                }
                else if (minDistSqr > Vector3.SqrMagnitude(transform.position - col.transform.position))
                {
                    nearestTarget = col;
                    minDistSqr = Vector3.SqrMagnitude(transform.position - col.transform.position);
                }
            }
        }
        if (!flag)
        {
            if (nearestTarget == null)
            {
                if (nowState != State.Wander && nowState != State.Chase)
                    target = wayPoints[Random.Range(0, wayPoints.Length - 1)];
                else if(target && Vector3.SqrMagnitude(transform.position - target.position) < 0.2f)
                    target = wayPoints[Random.Range(0, wayPoints.Length - 1)];
                agent.stoppingDistance = 0;
                nowState = State.Wander;
            }
            else
            {
                nowState = State.Chase;
                target = nearestTarget.transform;
                agent.stoppingDistance = 0;
            }
        }

        if (target == player && delay <= 0)
        {
            delay = 3;
            StartCoroutine(UseSkill(Random.Range(0, 3), 6, transform.position + Vector3.up * 0.3f, transform.eulerAngles.y));
            if (Vector3.SqrMagnitude(transform.position - target.position) > 30 && Random.Range(0, 100) < 50)
            {
                isWait = true;
                target = wayPoints[Random.Range(0, wayPoints.Length - 1)];
                agent.stoppingDistance = 0;
                nowState = State.Wander;
                Invoke("ReChase", 5f);
            }
        }

        if (delay > 0)
            delay -= Time.deltaTime;
    }

    IEnumerator UseSkill(int power, float range, Vector3 position, float rotation)
    {
        float startAngle;
        float angleGap;
        agent.speed = 0;
        if (power == 0)
        {
            yield return new WaitForSeconds(0.5f);
            delay = 4;
            startAngle = rotation - 30;
            angleGap = 4;
        }
        else
        {
            yield return new WaitForSeconds(0.35f);
            delay = 5f;
            startAngle = rotation - 26;
            angleGap = 3.5f;
        }

        for (int i = 0; i < 15; i++)
            Instantiate(bullet, position, Quaternion.Euler(0, startAngle + angleGap * i + Random.Range(-0.1f, 0.1f), 0));
        agent.speed = 2.5f;
    }

    public IEnumerator ResetDestination(float term)
    {
        while (true)
        {
            if (target)
                agent.SetDestination(target.position);
            yield return new WaitForSeconds(term);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up * sight.y * 0.5f, sight * 2);

        foreach(Transform point in wayPoints)
            Gizmos.DrawSphere(point.position, 0.5f);
    }
}
