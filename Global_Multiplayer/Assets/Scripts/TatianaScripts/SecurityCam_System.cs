using System.Collections.Generic;
using TMPro;
using UnityEngine;
/* Omogonix (2023), Basic Security Camera System in Unity - Unity C# Tutorial.[YouTube]
 * https://www.youtube.com/watch?v=ixnnXbzY688*/

public class SecurityCam_System : MonoBehaviour
{
    public List<GameObject> cameras;
    public int cameraSelected;
    public SecurityCam_System OtherButton;

    public TextMeshProUGUI Loctext;

    public void nextCam()
    {
        Debug.Log("works");
        cameraSelected = cameraSelected + 1;

        if (cameraSelected > cameras.Count - 1)
        {
            cameraSelected = 0;
        }

        if (cameraSelected > 0)
        {
            cameras[cameraSelected - 1].SetActive(false);
        }

        if (cameraSelected == 0)
        {
            cameras[cameras.Count - 1].SetActive(false);
        }

        cameras[cameraSelected].SetActive(true);
        OtherButton.cameraSelected = cameraSelected;
        Debug.Log(cameraSelected);
        Loctext.text = cameras[cameraSelected].name.ToString();
    }
    public void previousCam()
    {

        cameraSelected = cameraSelected - 1;

        if (cameraSelected < 0)
        {
            cameraSelected = cameras.Count - 1;
        }

        if (cameraSelected == cameras.Count - 1)
        {
            cameras[0].SetActive(false);
        }

        if (cameraSelected < cameras.Count - 1)
        {
            cameras[cameraSelected + 1].SetActive(false);
        }

        cameras[cameraSelected].SetActive(true);
        OtherButton.cameraSelected = cameraSelected;
        Debug.Log(cameraSelected);

        Loctext.text = cameras[cameraSelected].name.ToString();
    }
}
