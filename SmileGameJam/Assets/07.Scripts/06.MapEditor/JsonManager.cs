using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    public GameData gameData
    {
        get
        {
            return LoadJsonFile<GameData>(FilePath);
        }
    }

    private string FilePath = "/GameData.json";

    private void Awake()
    {
        FilePath = Application.dataPath + FilePath;
    }

    public void SaveData(GameData data)
    {
        CreateJsonFile(FilePath, JsonUtility.ToJson(data, prettyPrint: false));
    }

    public GameData LoadData()
    {
        var data = LoadJsonFile<GameData>(FilePath);
        return data;
    }

    void CreateJsonFile(string filePath, string jsonData)
    {
        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    T LoadJsonFile<T>(string filePath)
    {
        FileStream fileStream = new FileStream(filePath, FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }
}
