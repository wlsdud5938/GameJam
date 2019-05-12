using System.Collections.Generic;
using UnityEngine;

namespace MapEditor
{
    [System.Serializable]
    public class MapData
    {
        public List<RoomData> smallRoomData;
        public List<RoomData> mediumRoomData;
        public List<RoomData> largeRoomData;
    }

    [System.Serializable]
    public class RoomData
    {
        public int width = 8;

        public List<ObstacleData> obstacleData;
        public List<MonsterData> monsterData;
    }

    [System.Serializable]
    public struct ObstacleData
    {
        public int index;

        public int x, z;
        public int rotation;

        public ObstacleData(int index, Vector3 position, int rotation)
        {
            this.index = index;

            x = Mathf.RoundToInt(position.x);
            z = Mathf.RoundToInt(position.z);

            this.rotation = rotation;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            else
            {
                ObstacleData t = (ObstacleData)obj;
                return ((x == t.x) && (z == t.z));
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [System.Serializable]
    public struct MonsterData
    {
        public int index;

        public int x, z;
        public int rotation;

        public MonsterData(int index, Vector3 position, int rotation)
        {
            this.index = index;

            x = Mathf.RoundToInt(position.x);
            z = Mathf.RoundToInt(position.z);

            this.rotation = rotation;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            else
            {
                MonsterData t = (MonsterData)obj;
                return ((x == t.x) && (z == t.z));
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}