using UnityEngine;
using Mirror;
public class SceneChanger : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChangeScene();
        }
    }
    // This should be called when the UI Button is pressed
    public void ChangeScene()
    {
        // Only the server (host) is allowed to change the scene
        if (NetworkServer.active)
        {
            Debug.Log("Pressed");
            // Change scene for everyone
            NetworkManager.singleton.ServerChangeScene("MascotAI");
        }
    }
}

