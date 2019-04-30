using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        private bool isAutoSave = false;

        [Header("Obstacle")]
        public Transform tileInventory;
        public Button objectTile;

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
            RoomSetting(8);

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

            if (Physics.Raycast(ray, out hit))
            {
                cursor.gameObject.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    PutObstacle(nowIndex, hit.point);
                }
                else if (!Input.GetMouseButton(0))
                {
                    Vector3 pos = new Vector3(Mathf.RoundToInt(hit.point.x), 0, Mathf.RoundToInt(hit.point.z));
                    cursor.position = pos;
                }
            }
            else
            {
                cursor.gameObject.SetActive(false);
            }


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

        public void TileSetting()
        {
            objectList = new GameObject[objectData.obstacleList.Length];

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
                objectList[i].transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1,0,0,0.2f);
                if (i != 0) objectList[i].SetActive(false);
            }
        }

        private void RoomSetting(int width)
        {
            Debug.Log("Room Size Changed");

            Destroy(mapParent.gameObject);
            mapParent = new GameObject("Map").transform;

            for (int x = -width / 2; x < width / 2; x++)
            {
                for (int y = -width / 2; y < width / 2; y++)
                {
                    GameObject newGrid = Instantiate(grid, new Vector3(x * 2 + 0.5f, 0, y * 2 + 0.5f), Quaternion.identity, mapParent);
                }
            }
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

        }


        #region UI
        public void OpenLayer()
        {
            if (isObjectLayerOpen)
            {
                objectLayer.anchoredPosition = new Vector2(-50, 0);
                arrow.localEulerAngles = new Vector3(0, 0, -270);
            }
            else
            {
                objectLayer.anchoredPosition = new Vector2(-50, 100);
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
            smallRoomData = jsonManager.LoadData().smallRoomData;
            mediumRoomData = jsonManager.LoadData().mediumRoomData;
            largeRoomData = jsonManager.LoadData().largeRoomData;

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
                        Instantiate(objectData.obstacleList[m.index],
                            new Vector3(m.x, 0, m.z), Quaternion.Euler(0, 90 * m.rotation, 0), objectParent);

                    for (int i = 0; i < smallRoomData.Count; i++) options.Add(i.ToString());
                    break;
                case 1:
                    foreach (ObstacleData o in mediumRoomData[subStage].obstacleData)
                        Instantiate(objectData.obstacleList[o.index],
                            new Vector3(o.x, 0, o.z), Quaternion.Euler(0, 90 * o.rotation, 0), objectParent);
                    foreach (MonsterData m in mediumRoomData[subStage].monsterData)
                        Instantiate(objectData.obstacleList[m.index],
                            new Vector3(m.x, 0, m.z), Quaternion.Euler(0, 90 * m.rotation, 0), objectParent);

                    for (int i = 0; i < mediumRoomData.Count; i++) options.Add(i.ToString());
                    break;
                case 2:
                    foreach (ObstacleData o in largeRoomData[subStage].obstacleData)
                        Instantiate(objectData.obstacleList[o.index],
                            new Vector3(o.x, 0, o.z), Quaternion.Euler(0, 90 * o.rotation, 0), objectParent);
                    foreach (MonsterData m in largeRoomData[subStage].monsterData)
                        Instantiate(objectData.obstacleList[m.index],
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
                    RoomSetting(8);
                    for (int i = 0; i < smallRoomData.Count; i++) options.Add(i.ToString());
                    break;
                case 1:
                    RoomSetting(14);
                    for (int i = 0; i < mediumRoomData.Count; i++) options.Add(i.ToString());
                    break;
                case 2:
                    RoomSetting(20);
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