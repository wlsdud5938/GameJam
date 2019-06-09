using UnityEngine;
using MapEditor;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public GameObject[] enterances,walls;
    private bool[] openWalls = new bool[4];
    public List<MonsterData> monsters = new List<MonsterData>();

    public bool isEntered = false, isCleared = false;

    public int nowStage = 1;
    public int nowWave = 0;
    public int monsterCount;

    private int obstacleCount;

    [Header("[Grid]")]
    public LayerMask unwalkableMask;    //장애물 레이어 마스크
    LayerMask walkableMask;
    public Vector2 gridWorldSize;       //맵 사이즈
    public int obstacleProximityPenalty = 10;   //장애물 우선 패널티
    public float nodeRadius;            //노드 크기
    float nodeDiameter; //지름
    int gridSizeX, gridSizeY;
    Node[,] grid;

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    public void Open(int r)
    {
        enterances[r].SetActive(true);
        walls[r].SetActive(false);
        openWalls[r] = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCleared && other.CompareTag("Player"))
            EnterTheRoom();
    }

    private void Awake()
    {
        obstacleCount = ObjectData.instance.obstacleList.Length;
    }

    private void Start()
    {
        //nodeDiameter = nodeRadius * 2;
        //gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        //gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        //CreateGrid();
    }

    private void Update()
    {
        if (isEntered)
        {
            if(monsterCount <= 0)
            {
                for(int i = 0; i < monsters.Count; i++)
                {
                    Debug.Log(monsters[i].wave);
                    if(monsters[i].wave == nowWave)
                    {
                        Monster newMonster = Instantiate(ObjectData.instance.monsterList[monsters[i].index - obstacleCount], 
                            new Vector3(monsters[i].x, 0, monsters[i].z)+ transform.GetChild(1).transform.position, Quaternion.Euler(0, 90 * monsters[i].rotation, 0) , transform.GetChild(1).transform);
                        newMonster.SetInfo(nowStage, this);
                        monsterCount++;
                    }
                }
                if (nowWave++ > 3)
                    ClearTheRoom();
            }
        }
    }

    public void EnterTheRoom()
    {
        isEntered = true;
        monsterCount = 0;

        for(int i = 0; i < 4; i++)
        {
            if (openWalls[i])
                walls[i].SetActive(true);
        }
        Debug.Log("I must...");
    }

    public void ClearTheRoom()
    {
        isEntered = false;
        isCleared = true;

        for (int i = 0; i < 4; i++)
        {
            if (openWalls[i])
                walls[i].SetActive(false);
        }
        Debug.Log("ClearTheRoom");
    }


    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        //좌측 하단
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //해당 노드 (위치 * 노드 지름 * 노드 반지름 => x * 1 + 0.5)
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                int movementPenalty = 0;

                Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, 100, unwalkableMask)) movementPenalty = obstacleProximityPenalty;
                else movementPenalty = 0;

                Debug.Log(movementPenalty);
                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
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
                if (x == 0 && y == 0)
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

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

    //    if (grid != null)
    //    {
    //        foreach (Node n in grid)
    //        {
    //            Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(0, obstacleProximityPenalty, n.movementPenalty));

    //            Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
    //            Gizmos.DrawCube(n.worldPosition, Vector3.one * nodeDiameter);
    //        }
    //    }
    //}
}