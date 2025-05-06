using UnityEngine;

public class MissingDialInteract : MonoBehaviour, IInteractible
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

    public GameObject missingDial;
    public GameObject dialBase;
    public Material rightMat;

    public Animator lockAnim;
    public GameObject currentPlayer;

    public bool Interact(Interactors interact)
    {
        currentPlayer = interact.gameObject;

        if (interact.gameObject.GetComponent<PlayerInventory>().hasDial == true)
        {
            examine = false;
            place = true;

            missingDial.GetComponent<MeshRenderer>().material = rightMat;

            lockAnim.SetBool("isOn", true);

            interact.gameObject.GetComponent<PlayerInventory>().dialFilled = true;
            interact.gameObject.GetComponent<PlayerInventory>().hasDial = false;
            interact.gameObject.GetComponent<PlayerInventory>().UpdateInventory();


            GetComponent<MeshCollider>().enabled = false;

            dialBase.GetComponent<MeshCollider>().enabled = true;


        }

        else if (interact.gameObject.GetComponent<PlayerInventory>().hasDial == false)
        {

            interact.gameObject.GetComponent<CommsManager>().InteractComment(text);

        }


            return false;
    }
}
