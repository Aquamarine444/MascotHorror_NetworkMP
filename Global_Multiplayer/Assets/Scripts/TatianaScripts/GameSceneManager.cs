using UnityEngine;
using Mirror;

public class GameSceneManager : MonoBehaviour
{
    void Update()
    {
        if (NetworkServer.connections.Count != 2)
        {
            NetworkManager.singleton.ServerChangeScene("UIOverload");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
