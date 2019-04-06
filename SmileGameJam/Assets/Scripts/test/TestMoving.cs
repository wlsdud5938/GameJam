using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMoving : MonoBehaviour
{
    public float speed = 5.0f;
    NavMeshAgent agent;

    public Transform target;

    public GameObject player;
    public GameObject spawnPoint;
    public GameObject[] points = new GameObject[9];

    float mindist = 99999.0f;
    float dist = 99999.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        target = player.transform;
    }

    private void CheckItem()
    {

    }

    void Update()
    {
        mindist = Mathf.Abs(Vector3.Distance(transform.position, player.transform.position));
        for (int i = 0; i < spawnPoint.transform.childCount; i++)
        {
            if (spawnPoint.transform.GetChild(i).GetComponent<CheckItem>().checkItem)
            {
                dist = Mathf.Abs(Vector3.Distance(transform.position, spawnPoint.transform.GetChild(i).GetComponent<CheckItem>().ReturnItem().position));
                if (mindist > dist)
                {
                    mindist = dist;
                    target = spawnPoint.transform.GetChild(i).GetComponent<CheckItem>().ReturnItem();
                }
            }
        }
        if (target == player.transform)
            agent.stoppingDistance = gameObject.GetComponent<UnitInfo>().stopDist;
        else
            agent.stoppingDistance = 0;
        agent.SetDestination(target.position);
    }

}
