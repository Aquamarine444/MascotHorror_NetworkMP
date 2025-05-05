using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] Image loadingBar;
    public string NextScene;
    public float Timer;
    public GameObject Button;

    public AsyncOperation loadLevel;

    private void Start()
    {
        //StartCoroutine(LoadNextLevel());
        Button.SetActive(false);
    }

    private void Update()
    {
        Timer += Time.deltaTime;

        if (loadingBar.fillAmount == 1.0f)
        {
            Button.SetActive(true);
        }
    }

    /*IEnumerator LoadNextLevel()
    {
        loadLevel = SceneManager.LoadSceneAsync(NextScene);

        //loadLevel.allowSceneActivation = false;

        while (Timer != 9)
        {
            loadingBar.fillAmount = Mathf.Clamp01(Timer / .9f);
            yield return null;
        }
    }*/

    /*public void ContinueToScene()
    {
        loadLevel.allowSceneActivation = true;
    }*/

    public void ChangeScene()
     {
         // Only the server (host) is allowed to change the scene
          if (NetworkServer.active)
          {
              loadLevel.allowSceneActivation = true;

              // Change scene for everyone
              NetworkManager.singleton.ServerChangeScene(NextScene);
          }
     }

    public void QuitGame()
    {
        Application.Quit();
    }
}

