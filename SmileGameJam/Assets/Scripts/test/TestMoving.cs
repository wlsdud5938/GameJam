using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMoving : MonoBehaviour
{

    public Transform goal;
    public float speed = 5.0f;
    NavMeshAgent nav;
    public GameObject player;
    public Transform target;
    public GameObject spawnPoint;

    float mindist = 99999.0f;
    float dist = 99999.0f;
    int i = 0;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        target = player.transform;
        mindist = Mathf.Abs(Vector3.Distance(transform.position, player.transform.position));
        for (i = 0; i < spawnPoint.transform.childCount; i++)
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
            nav.stoppingDistance = gameObject.GetComponent<UnitInfo>().attackRange;
        else
            nav.stoppingDistance = 0;
        nav.SetDestination(target.position);
    }

}
