using UnityEngine;

public class LidInteract : MonoBehaviour, IInteractible
{
    [SerializeField] private bool inspect;
    [SerializeField] private bool examine;
    [SerializeField] private bool trigger;
    [SerializeField] private GameObject examineCamera;
    public bool Inspect { get { return inspect; } }
    public bool Examine { get { return examine; } }
    public bool Trigger { get { return trigger; } }
    public GameObject ExamineCam { get { return examineCamera; } }

    public string text;

    public CommsManager cM;

    public bool isOpen;

    public bool Interact(Interactors interact)
    {
        if (isOpen)
        {
            gameObject.transform.parent.GetComponent<Animator>().SetBool("DoorOpen", false);
            isOpen = false;
        }

        else if (!isOpen)
        {
            gameObject.transform.parent.GetComponent<Animator>().SetBool("DoorOpen", true);
            isOpen = false;
        }

       
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
        return false;
    }
}
