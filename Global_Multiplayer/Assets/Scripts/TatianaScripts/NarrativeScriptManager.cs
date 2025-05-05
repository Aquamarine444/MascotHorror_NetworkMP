using UnityEngine;

public class NarrativeScriptManager : MonoBehaviour
{
    public GameObject Poster;
    public GameObject Article;

    public GameObject Narrative;
    public GameObject Text_001;
    public GameObject Text_002;

    public float Timer;
    public float Timer2;
    public float Timer3;
    public float Timer4;

    public GameObject Button;

    public GameObject Audio;
    public GameObject SecondAudio;

    public GameObject MommaNarrative;
    public GameObject MommaLoad;

    private void Start()
    {
        Audio.SetActive(false);
        SecondAudio.SetActive(true);
    }

    private void Update()
    {
        if (Timer !> 0)
        {
            Timer -= Time.deltaTime;
        }

        if (Timer <= 0)
        {
            Poster.SetActive(false);
            Article.SetActive(true);

            if (Timer2 !> 0)
            {
                Timer2 -= Time.deltaTime;
            }
             else if (Timer2 <= 0)
            {
                Article.SetActive(false);
                Narrative.SetActive(true);

                if (Timer3 !> 0)
                {
                    Timer3 -= Time.deltaTime;
                }
                 else if (Timer3 <= 0)
                {
                    Text_001.SetActive(false);
                    Text_002.SetActive(true);

                    if (Timer4 !> 0)
                    {
                        Timer4 -= Time.deltaTime;
                    }
                    else if (Timer4 <= 0)
                    {
                        Button.SetActive(true);
                    }
                }
            }


        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SecondAudio.SetActive(false);
            Audio.SetActive(true);

            MommaNarrative.SetActive(false);

            MommaLoad.SetActive(true);
        }

    }

    public void Continue()
    {
        SecondAudio.SetActive(false);
        Audio.SetActive(true);
    }
}
