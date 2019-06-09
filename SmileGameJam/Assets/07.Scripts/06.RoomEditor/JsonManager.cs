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
                return LoadData();
            }
        }
        public TextAsset jsonFile;

        private void Awake()
        {
            jsonFile = Resources.Load("MapData") as TextAsset;
        }

        public void SaveData(MapData data)
        {
            CreateJsonFile(JsonUtility.ToJson(data, prettyPrint: true));
        }

        public MapData LoadData()
        {
            byte[] data = jsonFile.bytes;
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonUtility.FromJson<MapData>(jsonData); 
        }

        void CreateJsonFile(string jsonData)
        {
            string path = Application.dataPath + "/Resources/MapData.json";
            if (Application.platform == RuntimePlatform.Android)
                path = Application.persistentDataPath + "/Resources/MapData.json";
            else if(Application.platform == RuntimePlatform.IPhonePlayer)
                path = Application.dataPath + "/Resources/MapData.json";

            FileStream fileStream = new FileStream(path, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }
    }
}