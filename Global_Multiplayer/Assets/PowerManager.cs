using NUnit.Framework;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public bool powerOn = false;
    public GameObject[] lights;

    private void Start()
    {
        powerOn = false;
    }

    private void Update()
    {
        if (powerOn)
        {
            foreach (GameObject light in lights)
                light.SetActive(true);
        }
    }
}
