using UnityEngine;

public class PCInteract : MonoBehaviour, IInteractible
{
    [SerializeField] private bool inspect;
    [SerializeField] private bool examine;
    [SerializeField] private bool trigger;
    [SerializeField] private bool place;
    [SerializeField] private GameObject examineCamera;
    public bool Inspect { get { return inspect; } }
    public bool Examine { get { return examine; } }
    public bool Trigger { get { return trigger; } }
    public bool Place { get { return place; } }
    public GameObject ExamineCam { get { return examineCamera; } }

    public string text;

    public CommsManager cM;


    public bool Interact(Interactors interact)
    {



        return false;
    }
}
