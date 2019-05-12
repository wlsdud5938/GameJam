using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enterance : MonoBehaviour {

    public LobbyManager lobbyManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lobbyManager.StartToGame();
        }
    }
}
