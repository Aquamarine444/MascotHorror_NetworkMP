using UnityEngine;

public class FuseSwitchInteract : MonoBehaviour, IInteractible
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

    public bool isOn;

    public bool Interact(Interactors interact)
    {
        // When turning off
        if (isOn)
        {
            gameObject.transform.parent.GetComponent<Animator>().SetBool("isOn", false);
            isOn = false;
        }

        // When turning on
        else if (!isOn)
        {
            gameObject.transform.parent.GetComponent<Animator>().SetBool("isOn", true);
            isOn = true;

            if (interact.gameObject.GetComponent<PlayerInventory>().hasDial == false)
            {

            }
        }


        //gameObject.GetComponent<MeshRenderer>().enabled = false;
        return false;
    }
}
