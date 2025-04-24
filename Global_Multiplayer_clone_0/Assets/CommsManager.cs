using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommsManager : MonoBehaviour
{
    public TMP_Text popupText;
    public GameObject popup;

    public TMP_Text interactHelpText;
    public GameObject InteractHelpPopup;

    public float popupDuration;

    private void Update()
    {
        
    }

    public void changepopup(string text)
    {
        popupText.text = text;

        DisplayPopup(0);
    }

    public void InteractComment(string text)
    {
        interactHelpText.text = text;
        DisplayPopup(1);
    }


    public void DisplayPopup(int idk)
    {

        if (idk == 0)
        {
            popup.SetActive(true);
            StartCoroutine(PopupWait());
        }

        if (idk == 1)
        {
            InteractHelpPopup.SetActive(true);
        }
        

    }

    IEnumerator PopupWait()
    {
        yield return new WaitForSeconds(popupDuration);
        popup.SetActive(false);
        InteractHelpPopup.SetActive(false);
    }
}
