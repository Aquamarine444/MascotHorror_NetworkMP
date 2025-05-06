using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

    public GameObject light;
    public Material lightOff;
    public Material lightOn;

    public TurnOnPower power;

    public GameObject player;

    public bool Interact(Interactors interact)
    {
        player = interact.gameObject;

        // When turning off
        if (isOn)
        {
            gameObject.transform.parent.GetComponent<Animator>().SetBool("isOn", false);
            isOn = false;
            power.fuseTrigger = false;

            if (interact.gameObject.GetComponent<PlayerInventory>().fuseFilled == true)
            {
                light.GetComponent<MeshRenderer>().material = lightOff;
              
            }
               
        }

        // When turning on
        else if (!isOn)
        {
            gameObject.transform.parent.GetComponent<Animator>().SetBool("isOn", true);
            isOn = true;

            

            if (interact.gameObject.GetComponent<PlayerInventory>().fuseFilled == false)
            {
                StartCoroutine(SwitchFlip());
            }

            else if (interact.gameObject.GetComponent<PlayerInventory>().fuseFilled == true)
            {
                Debug.Log("Power is on");
                light.GetComponent<MeshRenderer>().material = lightOn;
                //turn power on

                power.fuseTrigger = true;

            }

        }
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
        return false;
    }

    public void forceTurnOff()
    {
        Debug.Log("works");
        gameObject.transform.parent.GetComponent<Animator>().SetBool("isOn", false);

        isOn = false;
        power.fuseTrigger = false;

        if (player.GetComponent<PlayerInventory>().fuseFilled == true)
        {
            light.GetComponent<MeshRenderer>().material = lightOff;
        }
    }

    private IEnumerator SwitchFlip()
    {
        yield return new WaitForSeconds(.5f);

        gameObject.transform.parent.GetComponent<Animator>().SetBool("isOn", false);
        isOn = false;


        

    }
}
