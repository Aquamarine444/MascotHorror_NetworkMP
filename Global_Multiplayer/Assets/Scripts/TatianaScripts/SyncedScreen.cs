using UnityEngine;
using Mirror;

public class SyncedScreen : MonoBehaviour
{
    public DeathScript Player1;
    public DeathScript Player2;

    public GameObject DeathScreen;

    public GameObject PauseScreen;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Player1 = GetComponent<DeathScript>();
        Player2 = GetComponent<DeathScript>();
    }
    private void Update()
    {
        if (NetworkServer.active)
        {
            Debug.Log("Friends");
        }
        else if (!NetworkServer.active)
        {
            Debug.Log("Porblem");
        }

        if (Player1.Death || Player2.Death)
        {
            DeathScreen.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PauseScreen.SetActive(true);
        }
    }

    public void Afterlife()
    {
        NetworkManager.singleton.ServerChangeScene("UIOverload");
    }
}
