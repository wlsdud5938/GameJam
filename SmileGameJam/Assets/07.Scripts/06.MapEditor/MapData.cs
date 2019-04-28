using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<StageData> stages;
}

public class StageData
{
    public int index = 0;
    public int subIndex = 0;
}

[System.Serializable]
public class RoomData
{
    public int width = 10;

    public List<ObstacleData> obstacleData;
    public List<MonsterData> monsterData;
}

[System.Serializable]
public struct ObstacleData
{
    public int index;

    public int x, y;
    public int rotation;

    public ObstacleData(int index, Vector2 position, int rotation)
    {
        this.index = index;

        x = Mathf.RoundToInt(position.x);
        y = Mathf.RoundToInt(position.y);

        this.rotation = rotation;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        else
        {
            ObstacleData t = (ObstacleData)obj;
            return ((x == t.x) && (y == t.y));
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

    public int x, y;

    public MonsterData(int index, Vector2 position)
    {
        this.index = index;

        x = Mathf.RoundToInt(position.x);
        y = Mathf.RoundToInt(position.y);
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        else
        {
            MonsterData t = (MonsterData)obj;
            return ((x == t.x) && (y == t.y));
        }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}