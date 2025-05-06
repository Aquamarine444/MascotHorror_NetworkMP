using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class DoorManager : MonoBehaviour
{
    public TurnOnPower powerSystem;
    public int maxDisabledDoorsBeforeShutdown = 30;

    public List<GameObject> doorObjects = new List<GameObject>();

    

    public static DoorManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        CheckPowerStatus();
    }

    void CheckPowerStatus()
    {
        int disabledCount = 0;

        foreach (GameObject door in doorObjects)
        {
            if (!door.activeInHierarchy)
            {
                disabledCount++;
            }
        }

        if (disabledCount >= maxDisabledDoorsBeforeShutdown && powerSystem != null && powerSystem.IsPowerOn)
        {
            powerSystem.ShutDownPower();
        }
    }

    public void ReactivateAllDoors()
    {
        foreach (GameObject door in doorObjects)
        {
            if (!door.activeInHierarchy)
            {
                door.SetActive(true);
            }
        }
    }
    public void DeactivateAllDoors()
    {
        foreach (GameObject door in doorObjects)
        {
            Doors doorScript = door.GetComponent<Doors>();

            if (doorScript != null)
            {
                doorScript.ToggleDoor();
            }
            else
            {
                Debug.Log("Broke");
            }
        }
    }
    public void DisableDoors()
    {
        foreach (GameObject door in doorObjects)
        {
            door.SetActive(false);
        }
    }
}
