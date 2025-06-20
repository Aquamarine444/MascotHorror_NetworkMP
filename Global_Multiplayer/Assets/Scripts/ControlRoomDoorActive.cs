using UnityEngine;
using TMPro;
using Mirror;

public class ControlRoomDoorActive : NetworkBehaviour
{
    [Header("Raycast Settings")]
    public float interactDistance = 3f;
    public LayerMask interactableLayers;

    [Header("Outline & UI Prompt")]
    public Behaviour outlineComponent;
    public TMP_Text promptText;
    public string promptMessage = "Press E to interact";

    [Header("Door Settings")]
    public GameObject doorObject;
    public float moveDistance = 3.5f;
    public float moveSpeed = 2f;
    public AudioClip openClip;
    public AudioClip closeClip;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = false;
    private bool isMoving = false;
    private AudioSource audioSrc;

    public override void OnStartServer()
    {
        closedPos = doorObject.transform.localPosition;
        openPos = closedPos + Vector3.up * moveDistance;
    }

    void Start()
    {
        audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.playOnAwake = false;
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        bool isLooking = IsLookingAtInteractable();

        outlineComponent.enabled = isLooking;
        if (promptText)
        {
            promptText.enabled = isLooking;
            if (isLooking) promptText.text = promptMessage;
        }

        if (isLooking && Input.GetKeyDown(KeyCode.E) && !isMoving)
        {
            CmdToggleControlRoomDoor();
        }

        if (isMoving)
            AnimateDoor();
    }

    [Command(requiresAuthority = false)]
    void CmdToggleControlRoomDoor()
    {
        isOpen = !isOpen;
        RpcPlayDoorSound(isOpen);
        isMoving = true;
    }

    [ClientRpc]
    void RpcPlayDoorSound(bool open)
    {
        AudioClip clip = open ? openClip : closeClip;
        if (clip && audioSrc) audioSrc.PlayOneShot(clip);
    }

    void AnimateDoor()
    {
        Vector3 target = isOpen ? openPos : closedPos;
        doorObject.transform.localPosition = Vector3.MoveTowards(doorObject.transform.localPosition, target, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(doorObject.transform.localPosition, target) < 0.01f)
            isMoving = false;
    }

    bool IsLookingAtInteractable()
    {
        Camera cam = Camera.main;
        if (!cam) return false;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, interactDistance, interactableLayers))
            return hit.collider.gameObject == gameObject;
        return false;
    }
}
