using Mirror;
using UnityEngine;

public class MapInteract : NetworkBehaviour, IInteractible
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

    public bool Interact(Interactors interact)
    {
        return false;
    }
}
