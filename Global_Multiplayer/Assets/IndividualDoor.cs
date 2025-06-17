using UnityEngine;

public class IndividualDoor : MonoBehaviour
{
    public GameObject SecuredDoor;
    public PowerManager powerSystem;
    public float powerDoorCost = 5f;

    public void DoorOpen()
    {
        if (powerSystem != null && powerSystem.powerOn)
        {
            ToggleDoor();
            //powerSystem.SpendPower(powerDoorCost); // Use the method to deduct power

            Debug.Log("works");

        }
        else
        {
            Debug.Log("Power is off. Door cannot be used by player.");
        }
    }

    public void ToggleDoor()
    {
        Debug.Log("door works");
        SecuredDoor.SetActive(!SecuredDoor.activeSelf);
        Debug.Log("Toggled door state.");
    }
}
