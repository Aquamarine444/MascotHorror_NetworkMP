using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

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

    public void OnReturnToStart()
    {
        SceneManager.LoadScene("UIOverload");
    }
}
