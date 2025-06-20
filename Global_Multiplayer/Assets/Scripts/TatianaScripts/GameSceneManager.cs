using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public GameObject PausePanel;
    void Update()
    {
        /*if (NetworkServer.connections.Count != 2)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("UIOverload");
        }*/

        /*if (Input.GetKeyUp(KeyCode.X))
        {
            OnReturnToStart();
        }*/

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanel.SetActive(true);
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
