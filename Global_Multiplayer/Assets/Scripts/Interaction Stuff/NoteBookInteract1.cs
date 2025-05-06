using UnityEngine;

public class NoteBookInteract : MonoBehaviour, IInteractible
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
    private bool isOpen;

    public GameObject page;
    public bool Interact(Interactors interact)
    {
        if (!isOpen)
        {
            gameObject.transform.parent.GetComponent<Animator>().SetBool("isOpen", true);
            isOpen = true;

            GetComponent<BoxCollider>().enabled = false;
            page.GetComponent<BoxCollider>().enabled = true;

        }


           return false;

    }
}
