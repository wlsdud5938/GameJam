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
        List<Room> smallRoomList, mediumRoomList, largeRoomList;
        smallRoomList = new List<Room>(); 
        mediumRoomList = new List<Room>();
        largeRoomList = new List<Room>();    

        int count = Mathf.NextPowerOfTwo(smallRoomCount + mediumRoomCount + largeRoomCount + 4);
        int sqr = (int)Mathf.Sqrt(count);
        int[] rooms = new int[count];
        
        for (int i = 0; i < smallRoomCount ; i++)
            rooms[i] = 1;
        for (int i = 0; i < mediumRoomCount; i++)
            rooms[smallRoomCount + i] = 2;
        for (int i = 0; i < largeRoomCount ; i++)
            rooms[smallRoomCount + mediumRoomCount + i] = 3;

        for (int x = 0; x < count; x++)
        {
            int r = Random.Range(0, count);
            int temp = rooms[x];
            rooms[x] = rooms[ r];
            rooms[r] = temp;
        }

        //yield return new WaitForSeconds(4.5f);
        yield return null;

        for (int x = 0; x < count; x++)
        {
            position = new Vector3(x % sqr, 0, x / sqr) * 18;
            switch (rooms[x])
            {
                case 1:
                    smallRoomList.Add(RoomGenerate(Instantiate(smallRoomObj, position, Quaternion.identity, mapParent), 0));
                    corridorGenerator.AddVertex(position, 4);
                    break;
                case 2:
                    mediumRoomList.Add(RoomGenerate(Instantiate(mediumRoomObj, position, Quaternion.identity, mapParent), 1));
                    corridorGenerator.AddVertex(position, 5);
                    break;
                case 3:
                    largeRoomList.Add(RoomGenerate(Instantiate(largeRoomObj, position, Quaternion.identity, mapParent), 2));
                    corridorGenerator.AddVertex(position, 6);
                    break;
            }
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