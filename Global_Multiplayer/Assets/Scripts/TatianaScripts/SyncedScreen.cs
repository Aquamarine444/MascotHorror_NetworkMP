using UnityEngine;
using Mirror;

public class SyncedScreen : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
    }
}
