using Mirror;
using UnityEngine;

public class IndividualDoor : NetworkBehaviour
{
    public GameObject SecuredDoor;
    public PowerManager powerSystem;
    public float powerDoorCost = 5f;

    public GameObject light;
    public Material lightRed;
    public Material lightGreen;
    public Material lightOff;

    [SyncVar(hook = nameof(OnDoorStateChanged))]
    public bool doorOpen;

    private void Update()
    {
        UpdateLightAndForceClose();
    }

    public void DoorOpen()
    {
       

        CmdToggleDoor();
    }

    [Command]
    private void CmdToggleDoor()
    {
        doorOpen = !doorOpen;

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

        SecuredDoor.SetActive(!doorOpen);
    }

    void OnDoorStateChanged(bool oldVal, bool newVal)
    {
        SecuredDoor.SetActive(!newVal);
    }

    void UpdateLightAndForceClose()
    {
        if (powerSystem == null) return;

        if (powerSystem.powerOn)
        {
            light.GetComponent<MeshRenderer>().material = doorOpen ? lightGreen : lightRed;
        }
        else
        {
            light.GetComponent<MeshRenderer>().material = lightOff;

            if (doorOpen)
            {
                doorOpen = false;
                SecuredDoor.SetActive(true);
                if (isServer)
                    powerSystem.doorsOpen--;

                Debug.Log("Power lost - door forcefully closed.");
            }
        }
    }
}
