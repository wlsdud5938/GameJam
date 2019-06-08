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
                return LoadJsonFile<MapData>(FilePath(fileName));
            }
        }

        private string fileName = "MapData.json";

        private string FilePath(string filename)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(Path.Combine(path, "Documents"), filename);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                string path = Application.persistentDataPath + filename;
                return path;
            }
            else
            {
                string path = Application.dataPath;
                return Path.Combine(path, filename);
            }
        }

        public void SaveData(MapData data)
        {
            CreateJsonFile(FilePath(fileName), JsonUtility.ToJson(data, prettyPrint: true));
        }

        public MapData LoadData()
        {
            var data = LoadJsonFile<MapData>(FilePath(fileName));
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