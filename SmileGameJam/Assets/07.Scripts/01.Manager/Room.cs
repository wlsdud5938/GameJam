using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] enterances,walls;

    public void Open(int r)
    {
        enterances[r].SetActive(true);
        walls[r].SetActive(false);
    }
}