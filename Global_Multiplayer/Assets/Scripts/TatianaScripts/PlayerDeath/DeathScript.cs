using Mirror;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    public bool Death;

    public string RestartScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death"))
        {
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        // Only the server (host) is allowed to change the scene
        if (NetworkServer.active)
        {
            // Change scene for everyone
            NetworkManager.singleton.ServerChangeScene("DeathScene");
        }
    }

    public void RestartGame()
    {
            // Only the server (host) is allowed to change the scene
            if (NetworkServer.active)
            {
                // Change scene for everyone
                NetworkManager.singleton.ServerChangeScene(RestartScene);
            }        
    }
}
