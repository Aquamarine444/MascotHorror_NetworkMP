using UnityEngine;
using TMPro;

public class ControlRoomDoorActive : MonoBehaviour
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

    void Start()
    {
        if (doorObject != null)
        {
            closedPos = doorObject.transform.localPosition;
            openPos = closedPos + Vector3.up * moveDistance;
        }
        if (outlineComponent != null) outlineComponent.enabled = false;
        if (promptText != null) promptText.enabled = false;

        audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.playOnAwake = false;
    }

    void Update()
    {
        bool isLooking = IsLookingAtInteractable();

        // Outline & prompt
        if (outlineComponent != null)
            outlineComponent.enabled = isLooking;
        if (promptText != null)
        {
            promptText.enabled = isLooking;
            if (isLooking)
                promptText.text = promptMessage;
        }

        // Toggle open/close
        if (isLooking && Input.GetKeyDown(KeyCode.E) && !isMoving && doorObject != null)
        {
            isOpen = !isOpen;
            isMoving = true;

            // Play sound
            AudioClip clip = isOpen ? openClip : closeClip;
            if (clip != null)
                audioSrc.PlayOneShot(clip);
        }

        // Animate door
        if (isMoving)
        {
            Vector3 target = isOpen ? openPos : closedPos;
            doorObject.transform.localPosition = Vector3.MoveTowards(
                doorObject.transform.localPosition,
                target,
                moveSpeed * Time.deltaTime
            );
            if (Vector3.Distance(doorObject.transform.localPosition, target) < 0.01f)
            {
                doorObject.transform.localPosition = target;
                isMoving = false;
            }
        }
    }

    bool IsLookingAtInteractable()
    {
        Camera cam = Camera.main;
        if (cam == null) return false;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, interactDistance, interactableLayers))
            return hit.collider.gameObject == gameObject;
        return false;
    }
}
