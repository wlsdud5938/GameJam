using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditor : MonoBehaviour {

    public RectTransform objectLayer;
    public RectTransform arrow;
    private bool isObjectLayerOpen = false;

    [Header("Obstacle")]
    public Transform tileInventory;
    public Button objectTile;

    [Header("Map Grid")]
    public Transform mapParent;
    public GameObject grid;

    public int nowIndex = 0;


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

    private void Start()
    {
        TileSetting();
        RoomLoad();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                PutObstacle(nowIndex, hit.point);
            }
        }
    }

    public void TileSetting()
    {
        for (int i = 0; i < ObjectData.instance.obstacleList.Length; i++)
        {
            Button newTile = Instantiate(objectTile, tileInventory);
            int index = i;
            newTile.onClick.AddListener(() =>
            {
                nowIndex = index;
            });
            newTile.transform.GetChild(1).GetComponent<Text>().text = ObjectData.instance.obstacleList[index].name;
        }
    }


    public void RoomLoad()
    {
        RoomSetting(8);
    }

    private void RoomSetting(int width)
    {
        Destroy(mapParent.gameObject);
        mapParent = new GameObject("Map").transform;

        for (int x = -width / 2; x < width /2; x++)
        {
            for (int y = -width / 2; y < width / 2; y++)
            {
                GameObject newGrid = Instantiate(grid, new Vector3(x * 2, 0, y * 2), Quaternion.identity, mapParent);
            }
        }
    }

    private void PutObstacle(int id, Vector3 position)
    {
        Vector3 pos = new Vector3(Mathf.RoundToInt(position.x), 1, Mathf.RoundToInt(position.z));
        Instantiate(ObjectData.instance.obstacleList[id], pos, Quaternion.identity);
    }

    private void PutMonster(int id, Vector3 position)
    {

    }
}
