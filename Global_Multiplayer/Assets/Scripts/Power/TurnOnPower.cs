using UnityEngine;
using System.Collections;
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
    private bool hasActivatedFromFuse = false;

    void Start()
    {
        currentPower = powerLevel;
        SetLights(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = false;
        }
    }

    public void ShutDownPower()
    {
        currentPower = 0f;
        powerIsOn = false;
        SetLights(false);
        currentPower = powerLevel;
        Debug.Log("Power shut down due to too many doors being open.");
    }

    void Update()
    {
        // TESTING: Press V to simulate power usage
        if (Input.GetKeyDown(KeyCode.V))
        {
            SpendPower(5f);
        }

        // Turn power on when fuse is triggered (once)
        if (fuseTrigger && !hasActivatedFromFuse)
        {
            hasActivatedFromFuse = true;

            if (currentPower > 0f)
            {
                powerIsOn = true;
                SetLights(true);
                currentPower = powerLevel;
                DoorManager.Instance?.ReactivateAllDoors();
            }
        }

        float powerLostThisFrame = 0f;

        // Drain power if it's on
        if (powerIsOn)
        {
            powerLostThisFrame = powerDecreaseRate * Time.deltaTime;
            currentPower -= powerLostThisFrame;
            totalPowerLost += powerLostThisFrame;

            // Notify PowerMeter when every 10 power units are lost
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

                // Optional: auto recharge
                currentPower = powerLevel;
                Debug.Log("Power recharged to 100.");
            }
        }

        Debug.Log("Power Level: " + currentPower.ToString("F2"));

        // Extra safeguard
        if (currentPower <= 0f && powerIsOn)
        {
            currentPower = 0f;
            powerIsOn = false;
            SetLights(false);
            Debug.Log("Power depleted. Turning off.");
        }
    }

    void SetLights(bool state)
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(state);
        }
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
        }
    }
}
