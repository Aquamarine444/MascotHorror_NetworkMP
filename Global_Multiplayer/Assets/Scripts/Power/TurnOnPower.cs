using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TurnOnPower : MonoBehaviour
{
    public GameObject[] lights;
    private bool powerIsOn = false;
    private bool inReach = false;
    public bool IsPowerOn => powerIsOn;

    [Header("Power Settings")]
    public float powerLevel = 100f;
    public float currentPower = 100f;
    public float powerDecreaseRate;

    public bool fuseTrigger = false;

    private float totalPowerLost = 0f;
    private bool previousFuseTrigger = false;

    public FuseSwitchInteract fsI;


    void Start()
    {
        currentPower = powerLevel;
        SetLights(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            inReach = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            inReach = false;
    }

    public void ShutDownPower()
    {
        currentPower = 0f;
        powerIsOn = false;
        SetLights(false);
        currentPower = powerLevel;
        Debug.Log("Power shut down due to too many doors being open.");

        fsI.forceTurnOff();
    }

    void Update()
    {

        // Manual test input
        if (Input.GetKeyDown(KeyCode.V))
            SpendPower(5f);

        // Detect change in fuseTrigger
        if (fuseTrigger != previousFuseTrigger)
        {
            previousFuseTrigger = fuseTrigger;

            if (fuseTrigger)
            {
                // Fuse activated
                if (currentPower > 0f)
                {
                    powerIsOn = true;
                    SetLights(true);
                    DoorManager.Instance?.ReactivateAllDoors();
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
                powerIsOn = false;
                SetLights(false);
                Debug.Log("Power turned OFF from fuse reset.");
            }
        }


        // Power draining
        if (powerIsOn)
        {
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
                powerIsOn = false;
                SetLights(false);
                Debug.Log("Power depleted. Turning off.");

                fsI.forceTurnOff();

                // Optional auto-recharge
                /*currentPower = powerLevel;
                GetComponent<PowerMeter>()?.ResetMeter();
                Debug.Log("Power recharged to 100.");*/
            }
        }

        Debug.Log("Power Level: " + currentPower.ToString("F2"));
    }

    void SetLights(bool state)
    {
        foreach (GameObject light in lights)
            light.SetActive(state);
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
            powerIsOn = false;
            SetLights(false);
            currentPower = powerLevel;

            Debug.Log("Power depleted due to door usage.");

            fsI.forceTurnOff();
        }
    }
}
