using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyNetworkManager : MonoBehaviour
{
    public TMP_Text Counter;
    public TMP_Text Player1;
    public TMP_Text Player2;

    public NetworkManager Manager;

    public GameObject Button;

    public string NextScene;

    private void Start()
    {
        Manager = FindAnyObjectByType<NetworkManager>();
    }

    public void UpdatePlayerCount()
    {
        if (Manager != null)
        {
            Counter.text = Manager.numPlayers.ToString();
        }
    }

    private void Update()
    {
        UpdatePlayerCount();

        if (Manager.numPlayers == 1)
        {
            Player1.text = "Greg";
            Player2.text = null;
            Button.SetActive(false);
        }
        if (Manager.numPlayers == 2)
        {
            Player2.text = "Charlie";
            Button.SetActive(true);
        }
    }

    public void ChangeScene()
    {
        // Only the server (host) is allowed to change the scene
        if (NetworkServer.active)
        {
            //loadLevel.allowSceneActivation = true;

            // Change scene for everyone
            NetworkManager.singleton.ServerChangeScene(NextScene);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}  