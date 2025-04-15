using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenScript : MonoBehaviour
{
    public GameObject Panel;
    public Image Background;

    private void Start()
    {
        Background = Panel.GetComponent<Image>();
    }

    IEnumerator FirstChange()
    {
        
        yield return null;
    }
}
