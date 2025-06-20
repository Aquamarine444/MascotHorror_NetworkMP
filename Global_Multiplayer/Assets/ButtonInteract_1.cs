using UnityEngine;
using Mirror;

public class ButtonInteract_1 : NetworkBehaviour, IInteractible
{
    [SerializeField] private bool inspect;
    [SerializeField] private bool examine;
    [SerializeField] private bool trigger;
    [SerializeField] private bool place;
    [SerializeField] private GameObject examineCamera;

    public bool Inspect => inspect;
    public bool Examine => examine;
    public bool Trigger => trigger;
    public bool Place => place;
    public GameObject ExamineCam => examineCamera;

    public string text;

    public CommsManager cM;

    public IndividualDoor door;

    public bool Interact(Interactors interact)
    {

        // We use a Command to ask the server to open the door
        CmdTryOpenDoor();

        return false;
    }

    [Command(requiresAuthority = false)]
    void CmdTryOpenDoor()
    {

        if (door != null)
        {
            door.DoorOpen(); // This function is server-side, synced with SyncVars
        }
    }
}
