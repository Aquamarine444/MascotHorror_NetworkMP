using Mirror;
using UnityEngine;


public class NarrativeManagerSpawner : NetworkBehaviour
{
    public int ViewCounter = 0;
    public GameObject LobbyScreen;

    [Server]
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
                ViewCounter += 1;

                if (ViewCounter % 2 == 1)
                {
                    LobbyScreen.SetActive(false);
                }

                if (ViewCounter % 2 == 0)
                {
                    LobbyScreen?.SetActive(true);
                }
          
        }
    }
}

