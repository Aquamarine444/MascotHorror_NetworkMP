using UnityEngine;

public class MissingFuseInteract : MonoBehaviour, IInteractible
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

    public bool isOn;
    public int dialogTime;


    public GameObject missingFuse;
    public Material rightMat;

    public bool Interact(Interactors interact)
    {
        if (interact.gameObject.GetComponent<PlayerInventory>().hasFuse == true)
        {
            examine = false;
            place = true;


            interact.gameObject.GetComponent<CommsManager>().InteractComment(textTrue);
            missingFuse.GetComponent<MeshRenderer>().material = rightMat;

            GetComponent<Animator>().SetBool("isOn", true);

            interact.gameObject.GetComponent<PlayerInventory>().fuseFilled = true;
        }

        else if (interact.gameObject.GetComponent<PlayerInventory>().hasFuse == false)
        {
            interact.gameObject.GetComponent<CommsManager>().InteractComment(textFalse);
        }

        return false;

    }


}
