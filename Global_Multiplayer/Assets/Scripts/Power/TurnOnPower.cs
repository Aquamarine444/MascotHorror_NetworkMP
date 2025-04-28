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
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            if (currentPower > 0f) // <- This check blocks reactivation if power is 0
            {
                powerIsOn = !powerIsOn;
                SetLights(powerIsOn);

                if (powerIsOn)
                {
                    DoorManager.Instance?.ReactivateAllDoors();
                }
            }
        }


        if (powerIsOn)
        {
            currentPower -= powerDecreaseRate * Time.deltaTime;

            if (currentPower <= 0f)
            {
                currentPower = 0f;
                powerIsOn = false;
                SetLights(false);
                Debug.Log("Power depleted. Turning off.");

                // Optional: Recharge automatically
                currentPower = powerLevel;
                Debug.Log("Power recharged to 100.");
            }
        }

        Debug.Log("Power Level: " + currentPower.ToString("F2"));

        if (currentPower <= 0f)
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
