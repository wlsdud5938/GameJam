using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    public delegate void spawn(Vector3 position);
    public GameObject player;
    public float playTime = 0.0f;

    public bool isTest = false;

    [Header("Game Start")]
    private MapGenerator mapGenerator;
    private CameraManager cameraManager;
    public Camera uiCam;

    public static GameDirector instance;

    private void Awake()
    {
        instance = this;
        cameraManager = Camera.main.GetComponent<CameraManager>();
        mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
    }

    private void Start()
    {
        if (!isTest)
            GameStart();
    }

    private void Update()
    {
        playTime += Time.deltaTime;
    }

    public void GameStart()
    {
        spawn spawnEvent = new spawn(SpawnPlayer);
        StartCoroutine(mapGenerator.MapGenerate(spawnEvent));
    }
    
    public void SpawnPlayer(Vector3 position)
    {
        player = Instantiate(player, position, Quaternion.identity);
        player.transform.GetChild(1).GetComponent<Canvas>().worldCamera = uiCam;
        cameraManager.enabled = true;
        cameraManager.target = GameObject.Find("Player").transform;
    }
}
