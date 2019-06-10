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

    public AudioSource audio;
    public AudioClip nowClip;
    public AudioClip normal;
    public AudioClip[] battle;

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
        nowClip = normal;
        audio.clip = nowClip;
        audio.Play();
    }

    public void SpawnPlayer(Vector3 position)
    {
        player = Instantiate(player, position, Quaternion.identity);
        player.transform.GetChild(1).GetComponent<Canvas>().worldCamera = uiCam;
        cameraManager.enabled = true;
        cameraManager.target = GameObject.Find("Player").transform;
    }

    public void NormalBgm()
    {
        nowClip = normal;
        StartCoroutine(BgmChange(nowClip, battle[UnityEngine.Random.Range(0, battle.Length)]));
    }

    public void BattleBgm()
    {
        StartCoroutine(BgmChange(nowClip, normal));
    }

    public IEnumerator BgmChange(AudioClip start, AudioClip end)
    {
        float basic = audio.volume;
        for (float volume = basic; volume > 0; volume -= Time.deltaTime * 1.75f)
        {
            audio.volume = volume;
            yield return null;
        }
        nowClip = end;
        audio.clip = nowClip;
        audio.Play();
        for (float volume = 0; volume < basic; volume += Time.deltaTime * 1.75f)
        {
            audio.volume = volume;
            yield return null;
        }
        audio.volume = basic;
    }
}
