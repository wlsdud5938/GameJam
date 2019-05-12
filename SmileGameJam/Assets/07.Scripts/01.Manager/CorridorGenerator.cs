using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorGenerator : MonoBehaviour {

    private List<Vertex> vertexes = new List<Vertex>();

    public GameObject straightCorridor;
    public GameObject cornerCorridor;
    
    private Astar astar;
    private Vector2 gridLeftDown = Vector3.zero, gridRightUp = Vector3.zero;

    private void Awake()
    {
        astar = GetComponent<Astar>();
    }

    public void AddVertex(Vector3 position, float width) {
        vertexes.Add(new Vertex(position, width));

        Debug.DrawRay(position + new Vector3(width,0, width), Vector3.up * 10, Color.red, 10);
        Debug.DrawRay(position - new Vector3(width,0, width), Vector3.up * 10, Color.red, 10);
        if (gridLeftDown.x > position.x - width)
            gridLeftDown = new Vector2(position.x - width, gridLeftDown.y);
        if (gridLeftDown.y > position.z - width)
            gridLeftDown = new Vector2(gridLeftDown.x, position.z - width);

        if (gridRightUp.x < position.x + width)
            gridRightUp = new Vector2(position.x + width, gridRightUp.y);
        if (gridRightUp.y < position.z + width)
            gridRightUp = new Vector2(gridRightUp.x, position.z + width);
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
        astar.grid.CreateGrid(new Vector2(gridLeftDown.x, gridLeftDown.y), new Vector2(gridRightUp.x, gridRightUp.y));
        for (int i = 1; i < vertexes.Count; i++)
        {
            if (vertexes[i].connectVertex != null)
            {
                Vector3[] waypoints = astar.FindPath(vertexes[i].position, vertexes[i].connectVertex.position);
            }

            ////Corridor Generate
            //for (int i = 1; i < vertexes.Count; i++)
            //{
            //    if (vertexes[i].connectVertex != null)
            //    {
            //        float xDiff = Mathf.Sign(vertexes[i].connectVertex.position.x - vertexes[i].position.x);
            //        float yDiff = Mathf.Sign(vertexes[i].connectVertex.position.z - vertexes[i].position.z);
            //        float x;

            //        if (xDiff > 0)
            //        {
            //            for (x = vertexes[i].position.x; x < vertexes[i].connectVertex.position.x; x += 2)
            //            {
            //                GameObject newRoad =
            //                    Instantiate(straightCorridor, new Vector3(x, 0, vertexes[i].position.z), Quaternion.Euler(0, 0, 0));
            //                newRoad.transform.SetParent(transform);
            //            }
            //        }
            //        else
            //        {
            //            for (x = vertexes[i].position.x; x > vertexes[i].connectVertex.position.x; x -= 2)
            //            {
            //                GameObject newRoad =
            //                Instantiate(straightCorridor, new Vector3(x, 0, vertexes[i].position.z), Quaternion.Euler(0, 0, 0));
            //                newRoad.transform.SetParent(transform);
            //            }
            //        }

            //        if (yDiff > 0)
            //        {
            //            for (float y = vertexes[i].position.z; y < vertexes[i].connectVertex.position.z; y += 2)
            //            {
            //                if (y == vertexes[i].position.z)
            //                {
            //                    float rotation = xDiff > 0 ? 90 : 180;
            //                    GameObject newRoad =
            //                        Instantiate(cornerCorridor, new Vector3(x, 0, y), Quaternion.Euler(0, rotation, 0));
            //                    newRoad.transform.SetParent(transform);
            //                }
            //                else
            //                {
            //                    GameObject newRoad =
            //                        Instantiate(straightCorridor, new Vector3(x, 0, y), Quaternion.Euler(0, 90, 0));
            //                    newRoad.transform.SetParent(transform);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            for (float y = vertexes[i].position.z; y > vertexes[i].connectVertex.position.z; y -= 2)
            //            {
            //                if (y == vertexes[i].position.z)
            //                {
            //                    float rotation = xDiff > 0 ? 0 : 270;
            //                    GameObject newRoad =
            //                        Instantiate(cornerCorridor, new Vector3(x, 0, y), Quaternion.Euler(0, rotation, 0));
            //                    newRoad.transform.SetParent(transform);
            //                }
            //                else
            //                {
            //                    GameObject newRoad =
            //                    Instantiate(straightCorridor, new Vector3(x, 0, y), Quaternion.Euler(0, 90, 0));
            //                    newRoad.transform.SetParent(transform);
            //                }
            //            }
            //        }
            //    }
            //}
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
