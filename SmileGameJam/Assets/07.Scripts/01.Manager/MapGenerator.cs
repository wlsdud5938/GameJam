using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapEditor;

public class MapGenerator : MonoBehaviour {
    
    [Header("Map Data")]
    public Transform mapParent;
    public GameObject roomObj;
    public Vector2 leftDown, rightUp;

    [Range(0, 15)]
    public int smallRoomCount, mediumRoomCount,largeRoomCount;

    private Transform[] smallroomPosition, mediumroomPosition, largeroomPosition;
    private List<Room> smallRoomList, mediumRoomList, largeRoomList;
    private Transform startRoom;

    [Header("Object Data")]
    public GameObject grid;
    private GameObject[] obstacleList;
    private List<RoomData> smallRoomData, mediumRoomData, largeRoomData;

    [Header("Manager")]
    private JsonManager jsonManager;
    private ObjectData objectData;
    //private PrimAlgorithm primAlgorithm;

    private void Awake()
    {
        jsonManager = GetComponent<JsonManager>();
        objectData = GetComponent<ObjectData>();

        obstacleList = new GameObject[objectData.obstacleList.Length];
        for (int i = 0; i < obstacleList.Length; i++)
            obstacleList[i] = objectData.obstacleList[i];
    }

    private void Start()
    {
        LoadRoomData();
    }

    public IEnumerator MapGenerate(GameDirector.spawn spawnEvent)
    {
        Vector2 position = new Vector2();
        smallRoomList = new List<Room>(); smallroomPosition = new Transform[smallRoomCount * 2];
        mediumRoomList = new List<Room>(); mediumroomPosition = new Transform[mediumRoomCount * 2];
        largeRoomList = new List<Room>(); largeroomPosition = new Transform[largeRoomCount * 2];

        for (int i = 0; i < smallRoomCount * 2; i++)
        {
            position = new Vector3(Random.Range(leftDown.x, rightUp.x), Random.Range(leftDown.y, rightUp.y), 0);
            GameObject newObj = Instantiate(roomObj, position, Quaternion.identity);
            newObj.transform.localScale = new Vector2(8, 8) * 2;
            smallroomPosition[i] = newObj.transform;
        }
        for (int i = 0; i < largeRoomCount * 2; i++)
        {
            position = new Vector3(Random.Range(leftDown.x, rightUp.x), Random.Range(leftDown.y, rightUp.y), 0);
            GameObject newObj = Instantiate(roomObj, position, Quaternion.identity);
            newObj.transform.localScale = new Vector2(20, 20) * 2;
            largeroomPosition[i] = newObj.transform;
        }
        for (int i = 0; i < mediumRoomCount * 2; i++)
        {
            position = new Vector3(Random.Range(leftDown.x, rightUp.x), Random.Range(leftDown.y, rightUp.y), 0);
            GameObject newObj = Instantiate(roomObj, position, Quaternion.identity);
            newObj.transform.localScale = new Vector2(14, 14) * 2;
            mediumroomPosition[i] = newObj.transform;
        }

        yield return new WaitForSeconds(4.5f);

        for (int i = 0; i < smallRoomCount * 2; i++)
        {
            if (i < smallRoomCount)
            {
                position = new Vector2(Mathf.RoundToInt(smallroomPosition[i].transform.position.x), Mathf.RoundToInt(smallroomPosition[i].transform.position.y));
                smallRoomList.Add(RoomGenerate(position, new Vector2(8, 8), 0));
            }
            Destroy(smallroomPosition[i].gameObject);
        }
        for (int i = 0; i < mediumRoomCount * 2; i++)
        {
            if (i < mediumRoomCount)
            {
                position = new Vector2(Mathf.RoundToInt(mediumroomPosition[i].transform.position.x), Mathf.RoundToInt(mediumroomPosition[i].transform.position.y));
                mediumRoomList.Add(RoomGenerate(position, new Vector2(14, 14), 1));
            }
            Destroy(mediumroomPosition[i].gameObject);
        }
        for (int i = 0; i < largeRoomCount * 2; i++)
        {
            if (i < largeRoomCount)
            {
                position = new Vector2(Mathf.RoundToInt(largeroomPosition[i].transform.position.x), Mathf.RoundToInt(largeroomPosition[i].transform.position.y));
                largeRoomList.Add(RoomGenerate(position, new Vector2(20, 20), 2));
            }
            Destroy(largeroomPosition[i].gameObject);
        }
        startRoom = smallRoomList[0].obj.transform;
        spawnEvent(startRoom.position);

        smallroomPosition = null;
        mediumroomPosition = null;
        largeroomPosition = null;
    }

    private Room RoomGenerate(Vector2 position, Vector2 scale, int size)
    {
        int width = Mathf.RoundToInt(scale.x);
        int height = Mathf.RoundToInt(scale.y);

        GameObject newRoom = new GameObject("Room");
        newRoom.transform.position = new Vector3(position.x, 0, position.y);
        newRoom.transform.SetParent(mapParent);

        GameObject newGround = new GameObject("Ground");
        newGround.transform.SetParent(newRoom.transform);
        newGround.transform.localPosition = Vector3.zero;

        newGround.AddComponent<BoxCollider>().size = new Vector3(width * 2, 0.1f, width * 2);
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -width / 2; y < width / 2; y++)
            {
                Vector3 pos = newRoom.transform.position + new Vector3(x * 2 + 0.5f, 0, y * 2 + 0.5f);
                GameObject newGrid = Instantiate(grid, pos, Quaternion.identity, newGround.transform);
            }
        }

        GameObject newObstacle = new GameObject("Obstacles");
        newObstacle.transform.SetParent(newRoom.transform);
        newObstacle.transform.localPosition = Vector3.zero;
        ObstacleGenerate(newObstacle.transform, size);

        return new Room(newRoom);
    }

    public void ObstacleGenerate(Transform parent, int size)
    {
        switch (size)
        {
            case 0:
                int r = Random.Range(0, smallRoomData.Count);
                foreach (ObstacleData o in smallRoomData[r].obstacleData)
                    Instantiate(objectData.obstacleList[o.index], parent.position + new Vector3(o.x, 0, o.z), Quaternion.Euler(0, 90 * o.rotation, 0), parent);
                foreach (MonsterData m in smallRoomData[r].monsterData)
                    Instantiate(objectData.obstacleList[m.index], parent.position +  new Vector3(m.x, 0, m.z), Quaternion.Euler(0, 90 * m.rotation, 0), parent);
                break;
            case 1:
                r = Random.Range(0, mediumRoomData.Count);
                foreach (ObstacleData o in mediumRoomData[r].obstacleData)
                    Instantiate(objectData.obstacleList[o.index], parent.position + new Vector3(o.x, 0, o.z), Quaternion.Euler(0, 90 * o.rotation, 0), parent);
                foreach (MonsterData m in mediumRoomData[r].monsterData)
                    Instantiate(objectData.obstacleList[m.index], parent.position + new Vector3(m.x, 0, m.z), Quaternion.Euler(0, 90 * m.rotation, 0), parent);
                break;
            case 2:
                r = Random.Range(0, largeRoomData.Count);
                foreach (ObstacleData o in largeRoomData[r].obstacleData)
                    Instantiate(objectData.obstacleList[o.index], parent.position + new Vector3(o.x, 0, o.z), Quaternion.Euler(0, 90 * o.rotation, 0), parent);
                foreach (MonsterData m in largeRoomData[r].monsterData)
                    Instantiate(objectData.obstacleList[m.index], parent.position + new Vector3(m.x, 0, m.z), Quaternion.Euler(0, 90 * m.rotation, 0), parent);
                break;
        }
    }

    public void LoadRoomData()
    {
        smallRoomData = jsonManager.LoadData().smallRoomData;
        mediumRoomData = jsonManager.LoadData().mediumRoomData;
        largeRoomData = jsonManager.LoadData().largeRoomData;
    }
}

public class Room
{
    public GameObject obj;
    public int index = 0;
    public static int i = 0;

    public Room(GameObject obj)
    {
        index = ++i;
        this.obj = obj;
    }
}