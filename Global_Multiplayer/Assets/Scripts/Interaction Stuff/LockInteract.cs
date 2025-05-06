using UnityEngine;

public class LockInteract : MonoBehaviour, IInteractible
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

    public string textTrue;
    public string textFalse;

    public CommsManager cM;
    public GameObject DialPuzzle;

    public bool Interact(Interactors interact)
    {
        if (interact.gameObject.GetComponent<PlayerInventory>().dialFilled == true)
        {
            interact.gameObject.GetComponent<CommsManager>().InteractComment(textTrue);

            DialPuzzle.GetComponent<RotaryLock>().enabled = true;
        }

            return false;
    }

}
