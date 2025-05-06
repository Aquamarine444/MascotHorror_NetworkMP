using UnityEngine;
using UnityEngine.UI;

public class PowerMeter : MonoBehaviour
{
    public Image[] powerBars; // Assign these in the inspector from top to bottom or left to right

    private int currentBarIndex;

    private void Start()
    {
        RemoveAll(); // Start with no bars showing
    }

    public void StartMeter()
    {
        foreach (Image img in powerBars)
            img.enabled = true;

        currentBarIndex = powerBars.Length - 1; // Start from last bar
    }

    public void RemoveBar()
    {
        if (currentBarIndex >= 0)
        {
            powerBars[currentBarIndex].enabled = false;
            currentBarIndex--;
        }
    }

    public void ResetMeter()
    {
        foreach (Image img in powerBars)
            img.enabled = true;

        currentBarIndex = powerBars.Length - 1;
    }

    public void RemoveAll()
    {
        foreach (Image img in powerBars)
            img.enabled = false;

        currentBarIndex = -1;
    }
}
