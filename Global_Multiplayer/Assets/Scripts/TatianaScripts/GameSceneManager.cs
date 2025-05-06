using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    void Update()
    {
        if (NetworkServer.connections.Count != 2)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("UIOverload");
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            OnReturnToStart();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnReturnToStart()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("UIOverload");
    }
}
