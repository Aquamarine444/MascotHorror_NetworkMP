using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IndividualDoor : MonoBehaviour
{
    public GameObject SecuredDoor;
    public PowerManager powerSystem;
    public float powerDoorCost = 5f;

    public GameObject light;
    public Material lightRed;
    public Material lightGreen;
    public Material lightOff;

    // Renamed for clarity
    public bool doorOpen;

    public void DoorOpen()
    {
        if (powerSystem != null && powerSystem.powerOn)
        {
            ToggleDoor();
        }
        else
        {
            Debug.Log("Power is off. Door cannot be used by player.");
        }
    }

    private void Update()
    {
        if (powerSystem.powerOn)
        {
            if (doorOpen)
            {
                light.GetComponent<MeshRenderer>().material = lightGreen;
                
            }
            else
            {
                light.GetComponent<MeshRenderer>().material = lightRed;
            }
        }
        else // Power is off
        {
            light.GetComponent<MeshRenderer>().material = lightOff;

            if (doorOpen)
            {
                // Force door closed
                doorOpen = false;
                SecuredDoor.SetActive(true);
                powerSystem.doorsOpen--;
                Debug.Log("Power lost - door forcefully closed.");
            }
        }
    }


    public void ToggleDoor()
    {
        doorOpen = !doorOpen;

        // If the door is open, hide it (i.e., remove the physical door GameObject)
        SecuredDoor.SetActive(!doorOpen);

        if (doorOpen)
        {
            powerSystem.doorsOpen++;
            powerSystem.SpendPower(powerDoorCost);
            Debug.Log("Door opened.");
        }
        else
        {
            powerSystem.doorsOpen--;
            Debug.Log("Door closed.");
        }
    }
}
