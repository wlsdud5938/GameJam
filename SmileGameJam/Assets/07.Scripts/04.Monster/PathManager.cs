using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult>();

    static PathManager instance;
    Astar astar;

    private void Awake()
    {
        instance = this;
        astar = GetComponent<Astar>();
    }

    private void Update()
    {
        if (results.Count > 0) //큐에 내용이 있을때만
        {
            int itemsInQueue = results.Count;
            lock (results) //임계영역 처리
            {
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        //쓰레드에 연결할 델리게이트 생성
        ThreadStart threadStart = delegate
        {
            //instance.astar.FindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

    public void FinishedProcessingPath(PathResult result)
    {
        //길 찾기 끝나면 할짓
        lock (results) //임계영역 처리
        {
            results.Enqueue(result);
        }
    }
}

//길찾기 결과
public struct PathResult
{
    public Vector3[] path;
    public bool success; //성공
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}

//길찾기 정보
public struct PathRequest
{
    public Vector3 pathStart;   //시작 위치
    public Vector3 pathEnd;     //목표 위치
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
    {
        pathStart = start;
        pathEnd = end;
        this.callback = callback;
    }
}
