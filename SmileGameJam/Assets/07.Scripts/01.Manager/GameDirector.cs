using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    public delegate void spawn(Vector3 position);
    public Player player;
    public float playTime = 0.0f;

    [Header("Game Start")]
    private MapGenerator mapGenerator;
    private CameraManager cameraManager;

    public static GameDirector instance;

    private void Awake()
    {
        instance = this;
        cameraManager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
    }

    private void Start()
    {
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
        cameraManager.enabled = true;
        cameraManager.target = player.transform;
    }
}
