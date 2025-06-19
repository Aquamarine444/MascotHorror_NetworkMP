using NUnit.Framework;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public bool powerOn = false;
    public GameObject[] lights;

    [Header("Power Settings")]
    public float powerLevel = 100f;
    public float currentPower = 100f;
    public float powerDecreaseRate;

    public bool fuseTrigger = false;

    private float totalPowerLost = 0f;
    private bool previousFuseTrigger = false;

    public FuseSwitchInteract fsI;

    public int doorsOpen;


    private void Start()
    {
        currentPower = powerLevel;
        powerOn = false;
    }

    public void SpendPower(float amount)
    {
        currentPower -= amount;
        totalPowerLost += amount;

        while (totalPowerLost >= 10f)
        {
            totalPowerLost -= 10f;
            GetComponent<PowerMeter>()?.RemoveBar();
        }

        if (currentPower <= 0f)
        {
            currentPower = 0f;
            powerOn = false;
            currentPower = powerLevel;

            Debug.Log("Power depleted due to door usage.");

            //fsI.forceTurnOff();
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.V))
            SpendPower(5f);*/

        if (doorsOpen >=3)
        {
            powerOn = false;
        }

        if (powerOn)
        {
            foreach (GameObject light in lights)
                light.SetActive(true);

            float powerLostThisFrame = powerDecreaseRate * Time.deltaTime;
            currentPower -= powerLostThisFrame;
            totalPowerLost += powerLostThisFrame;

            while (totalPowerLost >= 10f)
            {
                totalPowerLost -= 10f;
                GetComponent<PowerMeter>()?.RemoveBar();
            }

            if (currentPower <= 0f)
            {
                currentPower = 0f;
                powerOn = false;
                // turn power off
                Debug.Log("Power depleted. Turning off.");

                //fsI.forceTurnOff();
            }

            Debug.Log("Power Level: " + currentPower.ToString("F2"));
        }

        else
        {
            foreach (GameObject light in lights)
                light.SetActive(false);
        }

        if (fuseTrigger != previousFuseTrigger)
        {
            previousFuseTrigger = fuseTrigger;

            if (fuseTrigger)
            {
                // Fuse activated
                if (currentPower > 0f)
                {
                    powerOn = true;
                    //DoorManager.Instance?.ReactivateAllDoors();
                    Debug.Log("Power turned ON from fuse.");

                    PowerMeter meter = GetComponent<PowerMeter>();
                    if (meter != null)
                    {
                        meter.ResetMeter();   // Ensure all bars are visible
                        meter.StartMeter();   // Initialize the meter logic
                    }
                }
            }
            else
            {
                // Fuse deactivated
                powerOn = false;
                Debug.Log("Power turned OFF from fuse reset.");
            }
        }
    }
}
