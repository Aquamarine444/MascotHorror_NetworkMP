using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommsManager : MonoBehaviour
{

    [SerializeField] private TMP_Text popupText;
    [SerializeField] private GameObject popup;

    [SerializeField] private TMP_Text interactHelpText;
    [SerializeField] private GameObject InteractHelpPopup;

    [SerializeField] private float popupDuration;

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
