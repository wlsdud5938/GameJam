using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MapEditor
{
    public class MapEditor : MonoBehaviour
    {
        public RectTransform objectLayer,arrow;
        public Dropdown dataDropdown, indexDropdown;
        private bool isObjectLayerOpen = false;

        public Transform cursor;
        private GameObject[] objectList;

        private int nowIndex = 0;
        private bool isAutoSave = true;

        [Header("Obstacle")]
        public Transform tileInventory;
        public Button objectTile;
        private int obstacleCount;

        [Header("Monster")]
        public RectTransform monsterEditor;
        public Dropdown waveDropdown;
        private bool isSelected = false;
        private Transform selectedMonster;
        public int selectedIndex;

        [Header("Map Grid")]
        public GameObject grid;
        public Transform mapParent,objectParent;

        [Header("Stage Data")]
        public int stage = 1;
        public int subStage = 1;
        public List<RoomData> smallRoomData,mediumRoomData,largeRoomData;

        [Header("Test Play")]
        private bool isTesting = false;

        public GameObject playMode;
        public GameObject editMode;

        public MapCamera mapCamera;
        public CameraManager cameraManager;
        public Button playButton, stopButton;

        [Header("Manager")]
        public JsonManager jsonManager;
        public ObjectData objectData;

        private void Awake()
        {
            jsonManager = GameObject.Find("JsonManager").GetComponent<JsonManager>();
        }

        private void Start()
        {
            TileSetting();
            RoomSetting(4);

            LoadRoomData();
        }

        private void Update()
        {
            if (isTesting)
            {
                cursor.gameObject.SetActive(false);
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if ( Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (isSelected && Input.GetMouseButtonDown(0))
                    CloseMonsterEditor();
                if (!isSelected)
                {
                    cursor.gameObject.SetActive(true);
                    if (hit.transform.CompareTag("Obstacle"))
                    {
                        cursor.gameObject.SetActive(false);
                        if (Input.GetMouseButtonDown(0))
                            RemoveObstacle(hit.transform);
                    }
                    else if (hit.transform.CompareTag("Monster"))
                    {
                        cursor.gameObject.SetActive(true);
                        if (Input.GetMouseButtonDown(0))
                            SelectMonster(hit.transform);
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (nowIndex < obstacleCount)
                                PutObstacle(nowIndex, hit.point);
                            else
                                PutMonster(nowIndex, hit.point);
                        }
                        else if (!Input.GetMouseButton(0))
                        {
                            Vector3 pos = new Vector3(Mathf.RoundToInt(hit.point.x), 0, Mathf.RoundToInt(hit.point.z));
                            cursor.position = pos;
                        }
                    }
                }
            }
            else
            {
                cursor.gameObject.SetActive(false);
            }

            if (!isSelected && !Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    objectList[nowIndex].SetActive(false);
                    nowIndex = (nowIndex + 1 >= objectList.Length) ? 0 : nowIndex + 1;
                    objectList[nowIndex].SetActive(true);
                }
                else if (Input.mouseScrollDelta.y < 0)
                {
                    objectList[nowIndex].SetActive(false);
                    nowIndex = (nowIndex - 1 < 0) ? objectList.Length - 1 : nowIndex - 1;
                    objectList[nowIndex].SetActive(true);
                }
            }
        }

        public void SelectMonster(Transform monster)
        {
            isSelected = true;
            cursor.gameObject.SetActive(false);

            selectedMonster = monster;
            Vector3 pos = new Vector3(Mathf.RoundToInt(selectedMonster.position.x), 0, Mathf.RoundToInt(selectedMonster.position.z));
            MonsterData temp = new MonsterData(0, 0, pos, 0);
            selectedIndex = smallRoomData[subStage].monsterData.FindIndex(m => m.x == temp.x && m.z == temp.z);

            waveDropdown.value = smallRoomData[subStage].monsterData[selectedIndex].wave;
            monsterEditor.anchoredPosition = new Vector2(0, 0);
        }

        public void CloseMonsterEditor()
        {
            isSelected = false;
            selectedMonster = null;
            monsterEditor.anchoredPosition = new Vector2(100, 0);
        }

        public void SetWave(int wave)
        {
            MonsterData temp = smallRoomData[subStage].monsterData[selectedIndex];
            smallRoomData[subStage].monsterData[selectedIndex] = new MonsterData(temp.index, wave, new Vector3(temp.x, 0, temp.z), temp.rotation);

            if (isAutoSave)
                SaveMap();
        }

        public void RemoveMonster()
        {
            Vector3 pos = new Vector3(Mathf.RoundToInt(selectedMonster.position.x), 0, Mathf.RoundToInt(selectedMonster.position.z));
            MonsterData temp = new MonsterData(0, 0, pos, 0);

            switch (stage)
            {
                case 0:
                    smallRoomData[subStage].monsterData.Remove(temp);
                    break;
                case 1:
                    mediumRoomData[subStage].monsterData.Remove(temp);
                    break;
                case 2:
                    largeRoomData[subStage].monsterData.Remove(temp);
                    break;
            }

            Destroy(selectedMonster.gameObject);

            if (isAutoSave)
                SaveMap();
        }

        #region RoomSetting;
        public void TileSetting()
        {
            objectList = new GameObject[objectData.obstacleList.Length + objectData.monsterList.Length];

            for (int i = 0; i < objectData.obstacleList.Length; i++)
            {
                Button newTile = Instantiate(objectTile, tileInventory);
                int index = i;
                newTile.onClick.AddListener(() =>
                {
                    objectList[nowIndex].SetActive(false);
                    nowIndex = index;
                    objectList[nowIndex].SetActive(true);
                });
                newTile.transform.GetChild(1).GetComponent<Text>().text = objectData.obstacleList[index].name;

                objectList[i] = Instantiate(objectData.obstacleList[i], cursor);
                objectList[i].transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.2f);
                objectList[i].GetComponent<BoxCollider>().enabled = false;
                if (i != 0) objectList[i].SetActive(false);
            }

            obstacleCount = objectData.obstacleList.Length;
            for (int i = 0; i < objectData.monsterList.Length; i++)
            {
                Button newTile = Instantiate(objectTile, tileInventory);
                int index = i + obstacleCount;
                newTile.onClick.AddListener(() =>
                {
                    objectList[nowIndex].SetActive(false);
                    nowIndex = index;
                    objectList[nowIndex].SetActive(true);
                    Debug.Log(index);
                });
                newTile.transform.GetChild(1).GetComponent<Text>().text = objectData.monsterList[i].name;

                objectList[i + obstacleCount] = Instantiate(objectData.monsterList[i], cursor);
                objectList[i + obstacleCount].transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.2f);
                objectList[i + obstacleCount].GetComponent<BoxCollider>().enabled = false;
                objectList[i + obstacleCount].SetActive(false);
            }
        }

        private void RoomSetting(int width)
        {
            Debug.Log("Room Size Changed");

            Destroy(mapParent.gameObject);
            mapParent = new GameObject("Map").transform;

            int start = width % 2 == 0 ? width / 2 : width / 2 + 1;
            int end = width % 2 == 0 ? width / 2 : width / 2;

            for (int x = -start; x < end; x++)
            {
                for (int y = -start; y < end; y++)
                {
                    GameObject newGrid = Instantiate(grid, new Vector3(x * 2 + 0.5f, 0, y * 2 + 0.5f), Quaternion.identity, mapParent);
                }
            }
            if (width % 2 == 0)
                transform.GetComponent<BoxCollider>().center = new Vector3(-0.5f, 0, -0.5f);
            else
                transform.GetComponent<BoxCollider>().center = new Vector3(-1.5f, 0, -1.5f);
            transform.GetComponent<BoxCollider>().size = new Vector3(width * 2, 0.05f, width * 2);
        }
        #endregion;

        private void RemoveObstacle(Transform hit)
        {
            Vector3 pos = new Vector3(Mathf.RoundToInt(hit.position.x), 0, Mathf.RoundToInt(hit.position.z));
            ObstacleData temp = new ObstacleData(0, pos, 0);

            switch (stage)
            {
                case 0:
                    smallRoomData[subStage].obstacleData.Remove(temp);
                    break;
                case 1:
                    mediumRoomData[subStage].obstacleData.Remove(temp);
                    break;
                case 2:
                    largeRoomData[subStage].obstacleData.Remove(temp);
                    break;
            }

            Destroy(hit.gameObject);

            if (isAutoSave)
                SaveMap();
        }

        private void PutObstacle(int id, Vector3 position)
        {
            Vector3 pos = new Vector3(Mathf.RoundToInt(position.x), 0, Mathf.RoundToInt(position.z));
            Instantiate(objectData.obstacleList[id], pos, Quaternion.identity, objectParent);

            switch (stage)
            {
                case 0:
                    smallRoomData[subStage].obstacleData.Add(new ObstacleData(id, pos, 0));
                    break;
                case 1:
                    mediumRoomData[subStage].obstacleData.Add(new ObstacleData(id, pos, 0));
                    break;
                case 2:
                    largeRoomData[subStage].obstacleData.Add(new ObstacleData(id, pos, 0));
                    break;
            }

            if (isAutoSave)
                SaveMap();
        }

        private void PutMonster(int id, Vector3 position)
        {
            Vector3 pos = new Vector3(Mathf.RoundToInt(position.x), 0, Mathf.RoundToInt(position.z));
            Instantiate(objectData.monsterList[id - obstacleCount], pos, Quaternion.identity, objectParent);

            switch (stage)
            {
                case 0:
                    smallRoomData[subStage].monsterData.Add(new MonsterData(id, 0, pos, 0));
                    break;
                case 1:
                    mediumRoomData[subStage].monsterData.Add(new MonsterData(id, 0, pos, 0));
                    break;
                case 2:
                    largeRoomData[subStage].monsterData.Add(new MonsterData(id, 0, pos, 0));
                    break;
            }

            if (isAutoSave)
                SaveMap();
        }

        #region UI
        public void OpenLayer()
        {
            if (isObjectLayerOpen)
            {
                objectLayer.anchoredPosition = new Vector2(0, 0);
                arrow.localEulerAngles = new Vector3(0, 0, -270);
            }
            else
            {
                objectLayer.anchoredPosition = new Vector2(0, 100);
                arrow.localEulerAngles = new Vector3(0, 0, -90);
            }

            isObjectLayerOpen = !isObjectLayerOpen;
        }

        public void SaveMap()
        {
            Debug.Log("Save");
            jsonManager.SaveData(new MapData
            {
                smallRoomData = smallRoomData,
                mediumRoomData = mediumRoomData,
                largeRoomData = largeRoomData,
            });
        }

        public void LoadRoomData()
        {
            Debug.Log("Load From JsonData");
            MapData mapData = jsonManager.LoadData();
            smallRoomData = mapData.smallRoomData;
            mediumRoomData = mapData.mediumRoomData;
            largeRoomData = mapData.largeRoomData;

            LoadMap();
        }
        #endregion

        #region Load
        public void LoadMap()
        {
            Debug.Log("Load Map");

            Destroy(objectParent.gameObject);
            objectParent = new GameObject("Objects").transform;

            List<string> options = new List<string>();
            switch (stage)
            {
                case 0:
                    foreach (ObstacleData o in smallRoomData[subStage].obstacleData)
                        Instantiate(objectData.obstacleList[o.index],
                            new Vector3(o.x, 0, o.z), Quaternion.Euler(0, 90 * o.rotation, 0), objectParent);
                    foreach (MonsterData m in smallRoomData[subStage].monsterData)
                        Instantiate(objectData.monsterList[m.index - obstacleCount],
                            new Vector3(m.x, 0, m.z), Quaternion.Euler(0, 90 * m.rotation, 0), objectParent);

                    for (int i = 0; i < smallRoomData.Count; i++) options.Add(i.ToString());
                    break;
                case 1:
                    foreach (ObstacleData o in mediumRoomData[subStage].obstacleData)
                        Instantiate(objectData.obstacleList[o.index],
                            new Vector3(o.x, 0, o.z), Quaternion.Euler(0, 90 * o.rotation, 0), objectParent);
                    foreach (MonsterData m in mediumRoomData[subStage].monsterData)
                        Instantiate(objectData.monsterList[m.index - obstacleCount],
                            new Vector3(m.x, 0, m.z), Quaternion.Euler(0, 90 * m.rotation, 0), objectParent);

                    for (int i = 0; i < mediumRoomData.Count; i++) options.Add(i.ToString());
                    break;
                case 2:
                    foreach (ObstacleData o in largeRoomData[subStage].obstacleData)
                        Instantiate(objectData.obstacleList[o.index],
                            new Vector3(o.x, 0, o.z), Quaternion.Euler(0, 90 * o.rotation, 0), objectParent);
                    foreach (MonsterData m in largeRoomData[subStage].monsterData)
                        Instantiate(objectData.monsterList[m.index - obstacleCount],
                            new Vector3(m.x, 0, m.z), Quaternion.Euler(0, 90 * m.rotation, 0), objectParent);

                    for (int i = 0; i < largeRoomData.Count; i++) options.Add(i.ToString());
                    break;
            }
            indexDropdown.ClearOptions();
            indexDropdown.AddOptions(options);
        }
        #endregion

        #region Test
        public void TestPlay()
        {
            isTesting = true;

            playMode.SetActive(true);
            editMode.SetActive(false);

            playButton.gameObject.SetActive(false);
            stopButton.gameObject.SetActive(true);

            mapCamera.enabled = false;
            cameraManager.enabled = true;
        }
        public void TestStop()
        {
            isTesting = false;

            playMode.SetActive(false);
            editMode.SetActive(true);

            playButton.gameObject.SetActive(true);
            stopButton.gameObject.SetActive(false);

            mapCamera.enabled = true;
            cameraManager.enabled = false;
        }

        #endregion

        public void AddStage()
        {
            List<string> options = new List<string>();
            switch (stage)
            {
                case 0:
                    smallRoomData.Add(new RoomData
                    {
                        obstacleData = new List<ObstacleData>(),
                        monsterData = new List<MonsterData>()
                    });
                    for (int i = 0; i < smallRoomData.Count; i++) options.Add(i.ToString());
                    break;
                case 1:
                    mediumRoomData.Add(new RoomData
                    {
                        obstacleData = new List<ObstacleData>(),
                        monsterData = new List<MonsterData>()
                    });
                    for (int i = 0; i < mediumRoomData.Count; i++) options.Add(i.ToString());
                    break;
                case 2:
                    largeRoomData.Add(new RoomData
                    {
                        obstacleData = new List<ObstacleData>(),
                        monsterData = new List<MonsterData>()
                    });
                    for (int i = 0; i < largeRoomData.Count; i++) options.Add(i.ToString());
                    break;
            }
            indexDropdown.ClearOptions();
            indexDropdown.AddOptions(options);

            SaveMap();
        }

        public void AutoSave(bool on)
        {
            isAutoSave = on;
        }

        public void SetStage(int stage)
        {
            this.stage = stage;

            List<string> options = new List<string>();
            switch (stage)
            {
                case 0:
                    RoomSetting(4);
                    for (int i = 0; i < smallRoomData.Count; i++) options.Add(i.ToString());
                    break;
                case 1:
                    RoomSetting(6);
                    for (int i = 0; i < mediumRoomData.Count; i++) options.Add(i.ToString());
                    break;
                case 2:
                    RoomSetting(7);
                    for (int i = 0; i < largeRoomData.Count; i++) options.Add(i.ToString());
                    break;
            }
            indexDropdown.ClearOptions();
            indexDropdown.AddOptions(options);

            LoadMap();
        }

        public void SetSubstage(int subStage)
        {
            this.subStage = subStage;

            LoadMap();
        }
    }
}