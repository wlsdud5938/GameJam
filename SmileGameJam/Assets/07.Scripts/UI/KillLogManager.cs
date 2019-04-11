using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillLogManager : MonoBehaviour
{
    private Queue<KillData> killDataQueue = new Queue<KillData>();
    private Queue<KillLog> killLogs = new Queue<KillLog>();

    public UnitInfo player, enemy;

    private KillLog nowLog;
    private float popTime = 0f;

    static KillLogManager _instance;
    public static KillLogManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<KillLogManager>();
                if (_instance == null)
                    Debug.LogError("There's no KillLogManager");
            }
            return _instance;
        }
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            killLogs.Enqueue(transform.GetChild(i).GetComponent<KillLog>());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            AddKillData(player, enemy);

        if (popTime > 0)
            popTime -= Time.deltaTime;
        else
        {
            if (nowLog)
            {
                nowLog.HideLog();
                killLogs.Enqueue(nowLog);
                nowLog = null;
            }
            if (killDataQueue.Count > 0)
            {
                KillData newData = killDataQueue.Dequeue();
                nowLog = killLogs.Dequeue();
                nowLog.ShowLog(newData.killing, newData.killed);
                popTime = 3;
            }
        }
    }

    public void AddKillData(UnitInfo killing, UnitInfo killed)
    {
        killDataQueue.Enqueue(new KillData(killing, killed));
    }

    private struct KillData
    {
        public UnitInfo killing, killed;

        public KillData(UnitInfo killing, UnitInfo killed)
        {
            this.killing = killing;
            this.killed = killed;
        }
    }
}