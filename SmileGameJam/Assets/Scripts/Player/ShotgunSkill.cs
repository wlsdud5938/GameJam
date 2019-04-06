using System;
using UnityEngine;

public class ShotgunSkill : SkillBase
{
    private GameObject[] rangeList = new GameObject[5];
    public int[] angleList = new int[5];
    public float[] rangeNumList = new float[5];
    public Material material;

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject newObj = new GameObject("range" + i);
            mesh = new Mesh();
            newObj.transform.parent = transform;
            newObj.transform.localPosition = Vector3.zero;
            newObj.transform.localRotation = Quaternion.Euler(-90, 180, 0);
            newObj.AddComponent<MeshRenderer>();
            meshFilter = newObj.AddComponent<MeshFilter>();
            meshRenderer = newObj.GetComponent<MeshRenderer>();
            meshRenderer.material = material;
            ShowCircularSector(i, mesh, meshFilter);

            newObj.SetActive(false);
            rangeList[i] = newObj;
        }
    }

    public override void ShowRange(int power, Vector3 position, float rotation)
    {
        rangeList[power].transform.rotation = Quaternion.Euler(-90, 180 + rotation, 0);
        rangeList[power].transform.position = position;
        for (int i = 0; i < 5; i++)
        {
            if (i == power)
                rangeList[i].SetActive(true);
            else
                rangeList[i].SetActive(false);
        }
    }

    public override void HideRange()
    {
        for (int i = 0; i < 5; i++)
            rangeList[i].SetActive(false);
    }

    public override void UseSkill(int power, float range, Vector3 position, float rotation, Action hitCall)
    {
        float startAngle = rotation - angleList[power] * 0.5f;
        float angleGap = angleList[power] / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            BulletBase newBullet =
                BulletPooler.instance.ReuseObject(id, position, Quaternion.Euler(0, startAngle + angleGap * i + UnityEngine.Random.Range(-0.1f, 0.1f), 0));
            newBullet.SetInformation(damage, bulletSpeed + UnityEngine.Random.Range(-1.5f, 1.5f), range + UnityEngine.Random.Range(-0.3f, 0.2f));
        }
    }

    #region CircularSector Mesh Generate
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;

    public int intervalDegree = 5;
    private float beginCos, beginSin;
    private float endCos, endSin;
    private int beginNumber, endNumber;
    private int triangleNumber;

    public void ShowCircularSector(int index, Mesh mesh, MeshFilter meshFilter)
    {
        int currentIntervalDegree = Mathf.Abs(intervalDegree);

        int sectorCount = Mathf.Abs(angleList[index] / currentIntervalDegree);
        if (angleList[index] % intervalDegree != 0)
            ++sectorCount;

        mesh.Clear();
        vertices = new Vector3[sectorCount * 2 + 1];
        triangles = new int[sectorCount * 3];
        uvs = new Vector2[sectorCount * 2 + 1];
        vertices[0] = Vector3.zero;
        uvs[0] = new Vector2(0.5f, 0.5f);

        int i = 0;
        float beginDegree = 180 - angleList[index] * 0.5f;
        float limitDegree = beginDegree + angleList[index];// * 0.5f;
        float beginRadian, endRadian;
        float uvRadius = 0.5f;

        while (i < sectorCount)
        {
            float endDegree = beginDegree + currentIntervalDegree;

            if (endDegree > limitDegree)
                endDegree = limitDegree;

            beginRadian = Mathf.Deg2Rad * beginDegree;
            endRadian = Mathf.Deg2Rad * endDegree;

            beginCos = Mathf.Cos(beginRadian);
            beginSin = Mathf.Sin(beginRadian);
            endCos = Mathf.Cos(endRadian);
            endSin = Mathf.Sin(endRadian);

            beginNumber = i * 2 + 1;
            endNumber = i * 2 + 2;
            triangleNumber = i * 3;

            vertices[beginNumber].z = 0;
            vertices[beginNumber].y = -beginCos * rangeNumList[index];
            vertices[beginNumber].x = -beginSin * rangeNumList[index];
            vertices[endNumber].z = 0;
            vertices[endNumber].y = -endCos * rangeNumList[index];
            vertices[endNumber].x = -endSin * rangeNumList[index];

            triangles[triangleNumber] = 0;

            triangles[triangleNumber + 1] = endNumber;
            triangles[triangleNumber + 2] = beginNumber;

            uvs[beginNumber].x = -beginSin * uvRadius + 0.5f;
            uvs[beginNumber].y = -beginCos * uvRadius - 0.5f;
            uvs[endNumber].x = -endSin * uvRadius + 0.5f;
            uvs[endNumber].y = -endCos * uvRadius - 0.5f;

            beginDegree += currentIntervalDegree;
            ++i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.sharedMesh = mesh;
        meshFilter.sharedMesh.name = "CircularSectorMesh";
    }
    #endregion
}
