using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace MapEditor
{
    public class JsonManager : MonoBehaviour
    {
        public MapData mapData
        {
            get
            {
                return LoadJsonFile<MapData>(FilePath);
            }
        }

        private string FilePath = "/MapData.json";

        private void Awake()
        {
            FilePath = Application.dataPath + FilePath;
        }

        public void SaveData(MapData data)
        {
            CreateJsonFile(FilePath, JsonUtility.ToJson(data, prettyPrint: true));
        }

        public MapData LoadData()
        {
            var data = LoadJsonFile<MapData>(FilePath);
            return data;
        }

        void CreateJsonFile(string filePath, string jsonData)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
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
}