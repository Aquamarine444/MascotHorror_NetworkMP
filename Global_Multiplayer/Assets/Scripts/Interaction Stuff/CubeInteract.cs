using UnityEngine;

public class CubeInteract : MonoBehaviour, IInteractible
{
    [SerializeField] private bool inspect;
    [SerializeField] private bool examine;
    [SerializeField] private GameObject examineCamera;
    public bool Inspect { get { return inspect; } }
    public bool Examine { get { return examine; } }
    public GameObject ExamineCam { get { return examineCamera; } }

    public string text;

    public CommsManager cM;

    private void Start()
    {
        //cM = GameObject.FindWithTag("DialogueManager").GetComponent<CommsManager>();
    }

    public bool Interact(Interactors interact)
    {
        interact.gameObject.GetComponent<CommsManager>().InteractComment(text);

        //cM.InteractComment(text);
        return false;

    }

}
