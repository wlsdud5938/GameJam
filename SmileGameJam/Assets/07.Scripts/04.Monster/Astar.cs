using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour {

    public void FindPath(PathRequest request, Action<PathResult> callback, Room grid)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(request.pathStart);
        Node targetNode = grid.NodeFromWorldPoint(request.pathEnd);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);  //열린 노드 힙 생성
            HashSet<Node> closedSet = new HashSet<Node>();      //닫힌 노드 해쉬셋 생성
            openSet.Add(startNode);     //시작 노드를 열린 노드에 추가

            Node currentNode = null;
            while (openSet.Count > 0) //열린 노드가 없을때까지
            {
                currentNode = openSet.RemoveFirst(); //현 노드는 열린 노드 힙 첫 번째로 뽑아온 걸로
                closedSet.Add(currentNode); //닫힌 노드에 추가

                if (currentNode.Equals(targetNode)) //현 노드가 타겟이면
                {
                    pathSuccess = true; //탐색 완료
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode)) //주변 노드 탐색
                {
                    //장애물이거나 닫힌 노드일경우 탐색 안함
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;

                    //현 노드를 거쳐서 갈때의 코스트
                    int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                    //현 노드를 거쳐서 가는 것이 더 좋거나 아직 탐색이 된적없는 노드 일경우
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        //노드 코스트 재계산 및 대입
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        //노드의 부모를 현 노드로
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))//열린 노드에 없었으면 추가
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour); //이미 있으면 정렬 돌려주기
                    }
                }
            }
        }
        //길 탐색 성공시
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode); //부모를 찾아가기
            pathSuccess = waypoints.Length > 0; //이러면 움직일 필요 없잖아
        }
        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
