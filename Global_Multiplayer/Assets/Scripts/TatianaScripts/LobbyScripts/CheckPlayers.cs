using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPlayers : MonoBehaviour
{
    public string RestartScene;

    private NetworkManager NManager;

    private void Start()
    {
        NManager = FindAnyObjectByType<NetworkManager>();
    }
    private void Update()
    {
        if (NManager.numPlayers == 1 || NManager.numPlayers == 0)
        {
            NetworkManager.singleton.StopHost();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(RestartScene);
        }

    }
}
