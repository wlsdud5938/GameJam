using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorGenerator : MonoBehaviour {

    private List<Vertex> vertexes = new List<Vertex>();

    public GameObject straightCorridor;
    public GameObject cornerCorridor;

    public void AddVertex(Room room, Vector3 position, float width) {
        vertexes.Add(new Vertex(room, position, width));
    }

    public void CorridorGenerate()
    {
        //Prim Algorithm
        Vertex nowVertex = vertexes[0];
        nowVertex.nowWeight = 0;

        for (int i = 0; i < vertexes.Count; i++)
        {
            for (int j = 0; j < vertexes.Count; j++)
            {
                if (!vertexes[j].isVisited)
                {
                    nowVertex = vertexes[j];
                    break;
                }
            }
            for (int j = 0; j < vertexes.Count; j++)
            {
                if (!vertexes[j].isVisited && nowVertex.nowWeight > vertexes[j].nowWeight)
                    nowVertex = vertexes[j];
            }
            nowVertex.isVisited = true;

            for (int j = 0; j < vertexes.Count; j++)
            {
                if (nowVertex == vertexes[j] || vertexes[j].isVisited)
                    continue;

                float dist = Vector3.SqrMagnitude(nowVertex.position - vertexes[j].position);
                if (dist < vertexes[j].nowWeight)
                {
                    vertexes[j].nowWeight = dist;
                    vertexes[j].connectVertex = nowVertex;
                }
            }
        }
        //Corridor Generate
        for (int i = 1; i < vertexes.Count; i++)
        {
            if (vertexes[i].connectVertex != null)
            {
                Debug.DrawLine(vertexes[i].position + Vector3.up * 2, vertexes[i].connectVertex.position + Vector3.up * 2, Color.blue, 20);

                Vector3 startPoint = vertexes[i].position;
                Vector3 endPoint = vertexes[i].connectVertex.position;

                int xDir = Mathf.RoundToInt(endPoint.x - startPoint.x);
                int yDir = Mathf.RoundToInt(endPoint.z - startPoint.z);

                if (Random.Range(0, 2) == 0)
                {
                    int x = 0;

                    if (xDir > 0)
                    {
                        for (; x < xDir; x += 2)
                        {
                            if (x < vertexes[i].width + 2) continue;
                            if (yDir == 0 && x > xDir - vertexes[i].connectVertex.width - 2) continue;
                            GameObject newRoad = Instantiate(straightCorridor, startPoint + new Vector3(x, 0, 0), Quaternion.identity);
                            newRoad.transform.SetParent(transform);
                        }
                    }
                    else
                    {
                        for (; x > xDir; x -= 2)
                        {
                            if (x > -vertexes[i].width - 2) continue;
                            if (yDir == 0 && x < xDir + vertexes[i].connectVertex.width + 2) continue;
                            GameObject newRoad = Instantiate(straightCorridor, startPoint + new Vector3(x, 0, 0), Quaternion.identity);
                            newRoad.transform.SetParent(transform);
                        }
                    }

                    bool isCorner = false;
                    if (yDir > 0)
                    {
                        if (xDir != 0 && yDir != 0)
                        {
                            if (xDir > 0)
                            {
                                GameObject newRoad = Instantiate(cornerCorridor, startPoint + new Vector3(x, 0, 0), Quaternion.Euler(0, 90, 0));
                                newRoad.transform.SetParent(transform);
                            }
                            else
                            {
                                GameObject newRoad = Instantiate(cornerCorridor, startPoint + new Vector3(x, 0, 0), Quaternion.Euler(0, 180, 0));
                                newRoad.transform.SetParent(transform);
                            }
                            isCorner = true;
                        }
                        for (int y = 2; y < yDir; y += 2)
                        {
                            if (!isCorner && y < vertexes[i].width + 2) continue;
                            if (y > yDir - vertexes[i].connectVertex.width - 2) continue;
                            GameObject newRoad = Instantiate(straightCorridor, startPoint + new Vector3(x, 0, y), Quaternion.Euler(0, 90, 0));
                            newRoad.transform.SetParent(transform);
                        }
                    }
                    else
                    {
                        if (xDir != 0 && yDir != 0)
                        {
                            if (xDir > 0)
                            {
                                GameObject newRoad = Instantiate(cornerCorridor, startPoint + new Vector3(x, 0, 0), Quaternion.Euler(0, 0, 0));
                                newRoad.transform.SetParent(transform);
                            }
                            else
                            {
                                GameObject newRoad = Instantiate(cornerCorridor, startPoint + new Vector3(x, 0, 0), Quaternion.Euler(0, 270, 0));
                                newRoad.transform.SetParent(transform);
                            }
                            isCorner = true;
                        }
                        for (int y = -2; y > yDir; y -= 2)
                        {
                            if (!isCorner && y > -vertexes[i].width - 2) continue;
                            if (y < yDir + vertexes[i].connectVertex.width + 2) continue;
                            GameObject newRoad = Instantiate(straightCorridor, startPoint + new Vector3(x, 0, y), Quaternion.Euler(0, 90, 0));
                            newRoad.transform.SetParent(transform);
                        }
                    }

                    if (xDir > 0)
                    {
                        vertexes[i].room.Open(2);
                        if (yDir > 0)
                            vertexes[i].connectVertex.room.Open(1);
                        else if (yDir < 0)
                            vertexes[i].connectVertex.room.Open(0);
                        else
                            vertexes[i].connectVertex.room.Open(3);
                    }
                    else if (xDir < 0)
                    {
                        vertexes[i].room.Open(3);
                        if (yDir > 0)
                            vertexes[i].connectVertex.room.Open(1);
                        else if (yDir < 0)
                            vertexes[i].connectVertex.room.Open(0);
                        else
                            vertexes[i].connectVertex.room.Open(2);
                    }
                    else
                    {
                        if (yDir > 0)
                        {
                            vertexes[i].room.Open(0);
                            vertexes[i].connectVertex.room.Open(1);
                        }
                        else
                        {
                            vertexes[i].room.Open(1);
                            vertexes[i].connectVertex.room.Open(0);
                        }
                    }
                }
                else
                {
                    int y = 0;

                    if (yDir > 0)
                    {
                        for (; y < yDir; y += 2)
                        {
                            if (y < vertexes[i].width + 2) continue;
                            if (xDir == 0 && y > yDir - vertexes[i].connectVertex.width - 2) continue;
                            GameObject newRoad = Instantiate(straightCorridor, startPoint + new Vector3(0, 0, y), Quaternion.Euler(0, 90, 0));
                            newRoad.transform.SetParent(transform);
                        }
                    }
                    else
                    {
                        for (; y > yDir; y -= 2)
                        {
                            if (y > -vertexes[i].width - 2) continue;
                            if (xDir == 0 && y < yDir + vertexes[i].connectVertex.width + 2) continue;
                            GameObject newRoad = Instantiate(straightCorridor, startPoint + new Vector3(0, 0, y), Quaternion.Euler(0, 90, 0));
                            newRoad.transform.SetParent(transform);
                        }
                    }

                    bool isCorner = false;
                    if (xDir > 0)
                    {
                        if (xDir != 0 && yDir != 0)
                        {
                            if (yDir > 0)
                            {
                                GameObject newRoad = Instantiate(cornerCorridor, startPoint + new Vector3(0, 0, y), Quaternion.Euler(0, 270, 0));
                                newRoad.transform.SetParent(transform);
                            }
                            else
                            {
                                GameObject newRoad = Instantiate(cornerCorridor, startPoint + new Vector3(0, 0, y), Quaternion.Euler(0, 180, 0));
                                newRoad.transform.SetParent(transform);
                            }
                            isCorner = true;
                        }
                        for (int x = 2; x < xDir; x += 2)
                        {
                            if (!isCorner && x < vertexes[i].width + 2) continue;
                            if (x > xDir - vertexes[i].connectVertex.width - 2) continue;
                            GameObject newRoad = Instantiate(straightCorridor, startPoint + new Vector3(x, 0, y), Quaternion.identity);
                            newRoad.transform.SetParent(transform);
                        }
                    }
                    else
                    {
                        if (xDir != 0 && yDir != 0)
                        {
                            if (yDir > 0)
                            {
                                GameObject newRoad = Instantiate(cornerCorridor, startPoint + new Vector3(0, 0, y), Quaternion.Euler(0, 0, 0));
                                newRoad.transform.SetParent(transform);
                            }
                            else
                            {
                                GameObject newRoad = Instantiate(cornerCorridor, startPoint + new Vector3(0, 0, y), Quaternion.Euler(0, 90, 0));
                                newRoad.transform.SetParent(transform);
                            }
                            isCorner = true;
                        }
                        for (int x = -2; x > xDir; x -= 2)
                        {
                            if (!isCorner && x > -vertexes[i].width - 2) continue;
                            if (x < xDir + vertexes[i].connectVertex.width + 2) continue;
                            GameObject newRoad = Instantiate(straightCorridor, startPoint + new Vector3(x, 0, y), Quaternion.identity);
                            newRoad.transform.SetParent(transform);
                        }
                    }

                    if (yDir > 0)
                    {
                        vertexes[i].room.Open(0);
                        if (xDir > 0)
                            vertexes[i].connectVertex.room.Open(3);
                        else if (xDir < 0)
                            vertexes[i].connectVertex.room.Open(2);
                        else
                            vertexes[i].connectVertex.room.Open(1);
                    }
                    else if (yDir < 0)
                    {
                        vertexes[i].room.Open(1);
                        if (xDir > 0)
                            vertexes[i].connectVertex.room.Open(3);
                        else if (xDir < 0)
                            vertexes[i].connectVertex.room.Open(2);
                        else
                            vertexes[i].connectVertex.room.Open(0);
                    }
                    else
                    {
                        if (xDir > 0)
                        {
                            vertexes[i].room.Open(2);
                            vertexes[i].connectVertex.room.Open(3);
                        }
                        else
                        {
                            vertexes[i].room.Open(3);
                            vertexes[i].connectVertex.room.Open(2);
                        }
                    }
                }
            }
        }
    }

    public class Vertex
    {
        public Vector3 position;
        public bool isVisited;

        public Vertex connectVertex;
        public Room room;
        public float nowWeight = float.MaxValue;
        public float width;

        public Vertex(Room room, Vector3 position, float width)
        {
            this.room = room;
            isVisited = false;

            this.width = width;
            this.position = position;
        }
    }
}
