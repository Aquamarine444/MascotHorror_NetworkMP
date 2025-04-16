using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenScript : MonoBehaviour
{
    public GameObject StartPanel;

    public Sprite StaticScreen;
    public Sprite FreddieScreen;
    public Sprite BonnieScreen;

    private void Start()
    {
        StartCoroutine(FirstChange());
    }

    IEnumerator FirstChange()
    {
        yield return new WaitForSecondsRealtime(20);
        StartPanel.GetComponent<Image>().sprite = StaticScreen;
        StartCoroutine(SecondChange());
    }

    IEnumerator SecondChange()
    {
        yield return new WaitForSecondsRealtime(5);
        StartPanel.GetComponent<Image>().sprite = FreddieScreen;
        StartCoroutine(ThirdChange());
    }

    IEnumerator ThirdChange()
    {
        yield return new WaitForSecondsRealtime(20);
        StartPanel.GetComponent<Image>().sprite = StaticScreen;
        StartCoroutine(FinalChange());
    }

    IEnumerator FinalChange()
    {
        yield return new WaitForSecondsRealtime(5);
        StartPanel.GetComponent<Image>().sprite = BonnieScreen;
        StartCoroutine(FirstChange());
    }


}
