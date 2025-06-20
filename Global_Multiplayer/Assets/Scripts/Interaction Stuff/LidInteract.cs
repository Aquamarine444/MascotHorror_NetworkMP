using UnityEngine;
using Mirror;

public class LidInteract : NetworkBehaviour, IInteractible
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

    [SyncVar(hook = nameof(OnLidStateChanged))]
    public bool isOpen;

    public bool Interact(Interactors interact)
    {

        CmdToggleLid();

        return false;
    }

    [Command(requiresAuthority = false)]
    void CmdToggleLid()
    {
        isOpen = !isOpen;
    }

    // Called on clients when isOpen changes
    void OnLidStateChanged(bool oldState, bool newState)
    {
        Animator anim = gameObject.transform.parent.GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("DoorOpen", newState);
    }
}
