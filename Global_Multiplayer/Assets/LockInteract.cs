using UnityEngine;

public class LockInteract : MonoBehaviour, IInteractible
{
    [SerializeField] private bool inspect;
    [SerializeField] private bool examine;
    [SerializeField] private bool trigger;
    [SerializeField] private GameObject examineCamera;
    [SerializeField] private GameObject dial;
    public bool Inspect { get { return inspect; } }
    public bool Examine { get { return examine; } }
    public bool Trigger { get { return trigger; } }
    public GameObject ExamineCam { get { return examineCamera; } }

    public string textTrue;
    public string textFalse;

    public CommsManager cM;

    public bool Interact(Interactors interact)
    {
        if (interact.gameObject.GetComponent<PlayerInventory>().hasDial == true)
        {
            interact.gameObject.GetComponent<CommsManager>().InteractComment(textTrue);
            dial.SetActive(true);
        }

        else if (interact.gameObject.GetComponent<PlayerInventory>().hasDial == false)
        {
            interact.gameObject.GetComponent<CommsManager>().InteractComment(textFalse);
        }

            return false;
    }

}
