using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agrid : MonoBehaviour
{
    public bool displayGridGizmos;

    public LayerMask unwalkableMask;    //장애물 레이어 마스크

    public Vector2 gridLeftDown, gridRightUp;
    public Vector2 gridWorldSize;       //맵 사이즈
    public float nodeRadius;            //노드 크기
    public TerrainType[] walkableRegions;
    public int obstacleProximityPenalty = 10;   //장애물 우선 패널티
    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>(); //해당 레이어, 해당 레이어의 패널티

    Node[,] grid;

    float nodeDiameter; //지름
    int gridSizeX, gridSizeY;

    int penaltyMin = int.MaxValue, penaltyMax = int.MinValue;

    public Astar astar;

    void Awake()
    {
        foreach (TerrainType region in walkableRegions)
        {
            walkableMask.value |= region.terrainMask.value; //0010 + 0001 => 0011
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }
    }

    public void CreateGrid(Vector2 gridLeftDown, Vector2 gridRightUp)
    {
        nodeDiameter = nodeRadius * 2;
        this.gridLeftDown = gridLeftDown; this.gridRightUp = gridRightUp;

        gridWorldSize = new Vector2(gridRightUp.x - gridLeftDown.x, gridRightUp.y - gridLeftDown.y);
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        grid = new Node[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft = new Vector3(gridLeftDown.x, 0, gridLeftDown.y);
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                int movementPenalty = 0;

                Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, walkableMask))
                    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);

                if (!walkable)
                    movementPenalty += obstacleProximityPenalty; //아니면 장애물 패널티로

                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        //BlurPenaltyMap(3);
    }

    void BlurPenaltyMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] penaltyHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltyVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltyHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }
            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                penaltyHorizontalPass[x, y] = penaltyHorizontalPass[x - 1, y]
                    - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltyVerticalPass[x, 0] += penaltyHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltyVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeX; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                Debug.Log(gridSizeX + " : " + x);
                penaltyVerticalPass[x, y] = penaltyVerticalPass[x, y - 1] - penaltyHorizontalPass[x, removeIndex]
                    + penaltyHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltyVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            { 
                if (x != 0 && y != 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        if (grid == null)
            return null;
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Vector3 pos = new Vector3(gridLeftDown.x + gridRightUp.x, 0, gridLeftDown.y + gridRightUp.y);
        Gizmos.DrawWireCube(pos * 0.5f, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));

                Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
                Gizmos.DrawWireCube(n.worldPosition, Vector3.one * nodeDiameter);
            }
        }
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
