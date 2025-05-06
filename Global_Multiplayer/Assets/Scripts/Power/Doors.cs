using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class Doors : MonoBehaviour
{
    public GameObject SecuredDoor;
    public TurnOnPower powerSystem;
    public float powerDoorCost = 5f;

    private bool inReach = false;

    void Update()
    {
        if (inReach)
        {
            if (powerSystem != null && powerSystem.IsPowerOn)
            {
                ToggleDoor();
                powerSystem.SpendPower(powerDoorCost); // Use the method to deduct power
            }
            else
            {
                Debug.Log("Power is off. Door cannot be used by player.");
            }
        }
    }

    public void DoorOpen()
    {
        if (powerSystem != null && powerSystem.IsPowerOn)
        {
            ToggleDoor();
            powerSystem.SpendPower(powerDoorCost); // Use the method to deduct power

            Debug.Log("works");

        }
        else
        {
            Debug.Log("Power is off. Door cannot be used by player.");
        }
    }

    public void ToggleDoor()
    {
        SecuredDoor.SetActive(!SecuredDoor.activeSelf);
        Debug.Log("Toggled door state.");
    }

    public void ForceDisableAndEnable(float disableDelay, float enableDelay)
    {
        StartCoroutine(ForceDisableRoutine(disableDelay, enableDelay));
    }

    private IEnumerator ForceDisableRoutine(float disableDelay, float enableDelay)
    {
        yield return new WaitForSeconds(disableDelay);
        SecuredDoor.SetActive(false);
        Debug.Log("Animatronic disabled the door.");

        yield return new WaitForSeconds(enableDelay);
        SecuredDoor.SetActive(true);
        Debug.Log("Animatronic re-enabled the door.");
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

}
