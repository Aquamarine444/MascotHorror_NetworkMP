using UnityEngine;
using System.Collections.Generic;

public class TriggerAllPlayersCheck : MonoBehaviour
{
    [Tooltip("UI GameObject to activate when all players are inside")]
    public GameObject uiToActivate;

    private List<GameObject> playersInside = new List<GameObject>();
    private GameObject[] allPlayers;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiToActivate.SetActive(true);
            Time.timeScale = 0;

        }
    }
}

