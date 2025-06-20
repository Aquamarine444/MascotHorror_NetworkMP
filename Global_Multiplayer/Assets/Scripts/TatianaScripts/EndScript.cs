using Mirror;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    public string EndScene;

    public int Counter;

    private void Update()
    {
        if (Counter == 2)
        {
                if (NetworkServer.active)
                {
                    //loadLevel.allowSceneActivation = true;

                    // Change scene for everyone
                    NetworkManager.singleton.ServerChangeScene(EndScene);
                }           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Counter++;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Counter--;
        }
    
    }
}
