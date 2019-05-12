using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapEditor;

public class MapGenerator : MonoBehaviour {
    
    [Header("Map Data")]
    public Transform mapParent;
    public GameObject roomObj;
    public GameObject smallRoomObj, mediumRoomObj, largeRoomObj;
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
    private CorridorGenerator corridorGenerator;

    private void Awake()
    {
        jsonManager = GetComponent<JsonManager>();
        objectData = GetComponent<ObjectData>();
        corridorGenerator = GameObject.Find("CorriderGenerator").GetComponent<CorridorGenerator>();

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
        Vector3 position = new Vector3();
        smallRoomList = new List<Room>(); smallroomPosition = new Transform[smallRoomCount +3];
        mediumRoomList = new List<Room>(); mediumroomPosition = new Transform[mediumRoomCount + 3];
        largeRoomList = new List<Room>(); largeroomPosition = new Transform[largeRoomCount + 3];

        for (int i = 0; i < smallRoomCount +3; i++)
        {
            position = new Vector3(Random.Range(leftDown.x, rightUp.x), Random.Range(leftDown.y, rightUp.y), 0);
            GameObject newObj = Instantiate(roomObj, position, Quaternion.identity);
            newObj.transform.localScale = new Vector2(4, 4) * 2;
            smallroomPosition[i] = newObj.transform;
        }
        for (int i = 0; i < largeRoomCount +3; i++)
        {
            position = new Vector3(Random.Range(leftDown.x, rightUp.x), Random.Range(leftDown.y, rightUp.y), 0);
            GameObject newObj = Instantiate(roomObj, position, Quaternion.identity);
            newObj.transform.localScale = new Vector2(6, 6) * 2;
            largeroomPosition[i] = newObj.transform;
        }
        for (int i = 0; i < mediumRoomCount + 3; i++)
        {
            position = new Vector3(Random.Range(leftDown.x, rightUp.x), Random.Range(leftDown.y, rightUp.y), 0);
            GameObject newObj = Instantiate(roomObj, position, Quaternion.identity);
            newObj.transform.localScale = new Vector2(5, 5) * 2;
            mediumroomPosition[i] = newObj.transform;
        }

        yield return new WaitForSeconds(4.5f);

        for (int i = 0; i < smallRoomCount + 3; i++)
        {
            if (i < smallRoomCount)
            {
                position = new Vector3(Mathf.RoundToInt(smallroomPosition[i].transform.position.x),0, Mathf.RoundToInt(smallroomPosition[i].transform.position.y));
                smallRoomList.Add(RoomGenerate(Instantiate(smallRoomObj, position, Quaternion.identity, mapParent), 0));
                corridorGenerator.AddVertex(position,4);
            }
            Destroy(smallroomPosition[i].gameObject);
        }
        for (int i = 0; i < mediumRoomCount + 3; i++)
        {
            if (i < mediumRoomCount)
            {
                position = new Vector3(Mathf.RoundToInt(mediumroomPosition[i].transform.position.x),0, Mathf.RoundToInt(mediumroomPosition[i].transform.position.y));
                mediumRoomList.Add(RoomGenerate(Instantiate(mediumRoomObj, position, Quaternion.identity, mapParent), 1));
                corridorGenerator.AddVertex(position,5);
            }
            Destroy(mediumroomPosition[i].gameObject);
        }
        for (int i = 0; i < largeRoomCount + 3; i++)
        {
            if (i < largeRoomCount)
            {
                position = new Vector3(Mathf.RoundToInt(largeroomPosition[i].transform.position.x),0, Mathf.RoundToInt(largeroomPosition[i].transform.position.y));
                largeRoomList.Add(RoomGenerate(Instantiate(largeRoomObj, position, Quaternion.identity, mapParent), 2));
                corridorGenerator.AddVertex(position,6);
            }
            Destroy(largeroomPosition[i].gameObject);
        }
        startRoom = smallRoomList[0].obj.transform;

        corridorGenerator.CorridorGenerate();
        spawnEvent(startRoom.position);
    }

    private Room RoomGenerate(GameObject room, int size)
    {
        ObstacleGenerate(room.transform.GetChild(1).transform, size);
        return new Room(room);
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
    public Vector3 position;

    public Room(GameObject obj)
    {
        index = ++i;

        this.obj = obj;
    }
}