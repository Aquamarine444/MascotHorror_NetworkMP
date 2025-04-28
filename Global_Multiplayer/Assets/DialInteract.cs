using UnityEngine;

public class DialInteract : MonoBehaviour, IInteractible
{


    [SerializeField] private bool inspect;
    [SerializeField] private bool examine;
    [SerializeField] private GameObject examineCamera;
    public bool Inspect { get { return inspect; } }
    public bool Examine { get { return examine; } }
    public GameObject ExamineCam { get { return examineCamera; } }

    public string text;

    public CommsManager cM;

    public bool Interact(Interactors interact)
    {
        interact.gameObject.GetComponent<CommsManager>().InteractComment(text);
        interact.gameObject.GetComponent<PlayerInventory>().hasDial = true;
        interact.gameObject.GetComponent<PlayerInventory>().UpdateInventory();
        return false;
    }

}
