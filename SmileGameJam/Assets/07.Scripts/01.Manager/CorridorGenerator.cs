using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorGenerator : MonoBehaviour {

    private List<Vertex> vertexes = new List<Vertex>();

    public GameObject straightCorridor;
    public GameObject cornerCorridor;

    public void AddVertex(Vector3 position, float width) {
        vertexes.Add(new Vertex(position, width));
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
                float xDiff = Mathf.Sign(vertexes[i].connectVertex.position.x - vertexes[i].position.x);
                float yDiff = Mathf.Sign(vertexes[i].connectVertex.position.y - vertexes[i].position.y);
                float x;

                if (xDiff > 0)
                {
                    for (x = vertexes[i].position.x; x < vertexes[i].connectVertex.position.x; x += 2)
                    {
                        if (x > vertexes[i].position.x + vertexes[i].width)
                        {
                            GameObject newRoad =
                                Instantiate(straightCorridor, new Vector3(x, 0, vertexes[i].position.y), Quaternion.Euler(0, 0, 0));
                            newRoad.transform.SetParent(transform);
                        }
                    }
                }
                else
                {
                    for (x = vertexes[i].position.x; x > vertexes[i].connectVertex.position.x; x -= 2)
                    {
                        if (x < vertexes[i].position.x - vertexes[i].width)
                        {
                            GameObject newRoad =
                            Instantiate(straightCorridor, new Vector3(x, 0, vertexes[i].position.y), Quaternion.Euler(0, 0, 0));
                            newRoad.transform.SetParent(transform);
                        }
                    }
                }

                if (yDiff > 0)
                {
                    for (float y = vertexes[i].position.y; y < vertexes[i].connectVertex.position.y; y += 2)
                    {
                        if (y > vertexes[i].position.y + vertexes[i].width)
                        {
                            if (y == vertexes[i].position.y)
                            {
                                float rotation = xDiff > 0 ? 90 : 180;
                                GameObject newRoad =
                                    Instantiate(cornerCorridor, new Vector3(x, 0, y), Quaternion.Euler(0, rotation, 0));
                                newRoad.transform.SetParent(transform);
                            }
                            else
                            {
                                GameObject newRoad =
                                    Instantiate(straightCorridor, new Vector3(x, 0, y), Quaternion.Euler(0, 90, 0));
                                newRoad.transform.SetParent(transform);
                            }
                        }
                    }
                }
                else
                {
                    for (float y = vertexes[i].position.y; y > vertexes[i].connectVertex.position.y; y -= 2)
                    {
                        if (y < vertexes[i].position.y - vertexes[i].width)
                        {
                            if (y == vertexes[i].position.y)
                            {
                                float rotation = xDiff > 0 ? 0 : 270;
                                GameObject newRoad =
                                    Instantiate(cornerCorridor, new Vector3(x, 0, y), Quaternion.Euler(0, rotation, 0));
                                newRoad.transform.SetParent(transform);
                            }
                            else
                            {
                                GameObject newRoad =
                                Instantiate(straightCorridor, new Vector3(x, 0, y), Quaternion.Euler(0, 90, 0));
                                newRoad.transform.SetParent(transform);
                            }
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
        public float nowWeight = float.MaxValue;
        public float width;

        public Vertex(Vector3 position, float width)
        {
            isVisited = false;

            this.width = width;
            this.position = position;
        }
    }
}
