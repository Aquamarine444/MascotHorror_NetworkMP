using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenScript : MonoBehaviour
{
    public string ChangeTo;
    private void Start()
    {

    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(ChangeTo);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        // Only the server (host) is allowed to change the scene
        if (NetworkServer.active)
        {
            // Change scene for everyone
            NetworkManager.singleton.ServerChangeScene(ChangeTo);
        }
    }
}
