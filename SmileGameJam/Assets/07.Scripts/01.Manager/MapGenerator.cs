using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapEditor;
using System.IO;

public class MapGenerator : MonoBehaviour {
    
    [Header("Map Data")]
    public Transform mapParent;
    public Room smallRoomObj, mediumRoomObj, largeRoomObj;
    public Vector2 leftDown, rightUp;

    [Range(0, 15)]
    public int smallRoomCount, mediumRoomCount,largeRoomCount;
    
    private Transform startRoom;

    [Header("Object Data")]
    private GameObject[] obstacleList;
    private List<RoomData> smallRoomData, mediumRoomData, largeRoomData;
    private List<Room> smallRoomList, mediumRoomList, largeRoomList;

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
        smallRoomList = new List<Room>();
        mediumRoomList = new List<Room>();
        largeRoomList = new List<Room>();

        int count = Mathf.NextPowerOfTwo(smallRoomCount + mediumRoomCount + largeRoomCount + 4);
        int sqr = (int)Mathf.Sqrt(count);
        int[] rooms = new int[count];

        for (int i = 0; i < smallRoomCount; i++)
            rooms[i] = 1;
        for (int i = 0; i < mediumRoomCount; i++)
            rooms[smallRoomCount + i] = 2;
        for (int i = 0; i < largeRoomCount; i++)
            rooms[smallRoomCount + mediumRoomCount + i] = 3;

        for (int x = 0; x < count; x++)
        {
            int r = Random.Range(0, count);
            int temp = rooms[x];
            rooms[x] = rooms[r];
            rooms[r] = temp;
        }
        yield return null;

        for (int x = 0; x < count; x++)
        {
            position = new Vector3(x % sqr, 0, x / sqr) * 18;
            switch (rooms[x])
            {
                case 1:
                    Room newRoom = RoomGenerate(Instantiate(smallRoomObj, position, Quaternion.identity, mapParent), 0);
                    smallRoomList.Add(newRoom);
                    corridorGenerator.AddVertex(newRoom, position, 4);
                    break;
                case 2:
                    newRoom = RoomGenerate(Instantiate(mediumRoomObj, position, Quaternion.identity, mapParent), 1);
                    mediumRoomList.Add(newRoom);
                    corridorGenerator.AddVertex(newRoom, position, 6);
                    break;
                case 3:
                    newRoom = RoomGenerate(Instantiate(largeRoomObj, position, Quaternion.identity, mapParent), 2);
                    largeRoomList.Add(newRoom);
                    corridorGenerator.AddVertex(newRoom, position, 7);
                    break;
            }
        }

        startRoom = smallRoomList[0].transform;
        Room nextRoom = smallRoomList[0];
        float dist = 0;

        foreach (Room r in smallRoomList)
        {
            if (dist < Vector3.SqrMagnitude(r.transform.position - startRoom.position))
            {
                dist = Vector3.SqrMagnitude(r.transform.position - startRoom.position);
                nextRoom = r;
            }
        }
        foreach (Room r in mediumRoomList)
        {
            if (dist < Vector3.SqrMagnitude(r.transform.position - startRoom.position))
            {
                dist = Vector3.SqrMagnitude(r.transform.position - startRoom.position);
                nextRoom = r;
            }
        }
        foreach (Room r in largeRoomList)
        {
            if (dist < Vector3.SqrMagnitude(r.transform.position - startRoom.position))
            {
                dist = Vector3.SqrMagnitude(r.transform.position - startRoom.position);
                nextRoom = r;
            }
        }

        corridorGenerator.CorridorGenerate();
        spawnEvent(startRoom.position);
    }

    private Room RoomGenerate(Room room, int size)
    {
        if (smallRoomList.Count > 0)
            ObstacleGenerate(room, room.transform.GetChild(1).transform, size);
        else
            room.isCleared = true;
        return room;
    }

    public void ObstacleGenerate(Room room, Transform parent, int size)
    {
        int obstacleCount = objectData.obstacleList.Length;
        switch (size)
        {
            case 0:
                int r = Random.Range(0, smallRoomData.Count);
                foreach (ObstacleData o in smallRoomData[r].obstacleData)
                    Instantiate(objectData.obstacleList[o.index], parent.position + new Vector3(o.x, 0, o.z), Quaternion.Euler(0, 90 * o.rotation, 0), parent);
                foreach (MonsterData m in smallRoomData[r].monsterData)
                    room.monsters.Add(m);
                //    Instantiate(objectData.monsterList[m.index - obstacleCount], parent.position + new Vector3(m.x, 0, m.z), Quaternion.Euler(0, 90 * m.rotation, 0), parent);
                break;
            case 1:
                r = Random.Range(0, mediumRoomData.Count);
                foreach (ObstacleData o in mediumRoomData[r].obstacleData)
                    Instantiate(objectData.obstacleList[o.index], parent.position + new Vector3(o.x, 0, o.z), Quaternion.Euler(0, 90 * o.rotation, 0), parent);
                foreach (MonsterData m in mediumRoomData[r].monsterData)
                    room.monsters.Add(m);
                //    Instantiate(objectData.monsterList[m.index - obstacleCount], parent.position + new Vector3(m.x, 0, m.z), Quaternion.Euler(0, 90 * m.rotation, 0), parent);
                break;
            case 2:
                r = Random.Range(0, largeRoomData.Count);
                foreach (ObstacleData o in largeRoomData[r].obstacleData)
                    Instantiate(objectData.obstacleList[o.index], parent.position + new Vector3(o.x, 0, o.z), Quaternion.Euler(0, 90 * o.rotation, 0), parent);
                foreach (MonsterData m in largeRoomData[r].monsterData)
                    room.monsters.Add(m);
                //    Instantiate(objectData.monsterList[m.index - obstacleCount], parent.position + new Vector3(m.x, 0, m.z), Quaternion.Euler(0, 90 * m.rotation, 0), parent);
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