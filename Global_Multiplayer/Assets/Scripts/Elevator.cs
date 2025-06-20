using UnityEngine;
using Mirror;

public class Elevator : NetworkBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openElevatorClip;

    [Header("Visuals & Buttons")]
    public GameObject elevatorLight;
    public GameObject[] upOnButtons;
    public GameObject[] upOffButtons;

    [Header("Doors")]
    public Transform leftDoor;
    public Transform rightDoor;
    public float doorOffset = 0.00798f;
    public float doorMoveSpeed = 1f;

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;
    private bool isOpen = false;
    private bool isMoving = false;

    void Start()
    {
        // Cache door positions
        if (leftDoor != null) leftClosedPos = leftDoor.localPosition;
        if (rightDoor != null) rightClosedPos = rightDoor.localPosition;
        leftOpenPos = leftClosedPos + Vector3.down * doorOffset;
        rightOpenPos = rightClosedPos + Vector3.up * doorOffset;

        // Initial state: off lights and buttons
        if (elevatorLight != null) elevatorLight.SetActive(false);
        foreach (var btn in upOnButtons) if (btn != null) btn.SetActive(false);
        foreach (var btn in upOffButtons) if (btn != null) btn.SetActive(true);

        // Trigger elevator open on start
        OpenElevator();
    }

    void Update()
    {
        // Animate doors
        if (isMoving)
        {
            Vector3 targetLeft = isOpen ? leftOpenPos : leftClosedPos;
            Vector3 targetRight = isOpen ? rightOpenPos : rightClosedPos;

            if (leftDoor != null)
                leftDoor.localPosition = Vector3.MoveTowards(leftDoor.localPosition, targetLeft, doorMoveSpeed * Time.deltaTime);
            if (rightDoor != null)
                rightDoor.localPosition = Vector3.MoveTowards(rightDoor.localPosition, targetRight, doorMoveSpeed * Time.deltaTime);

            // Check if both doors reached targets
            bool leftDone = leftDoor == null || Vector3.Distance(leftDoor.localPosition, targetLeft) < 0.001f;
            bool rightDone = rightDoor == null || Vector3.Distance(rightDoor.localPosition, targetRight) < 0.001f;
            if (leftDone && rightDone)
                isMoving = false;
        }
    }

    private void OpenElevator()
    {
        // Set state
        isOpen = true;
        isMoving = true;

        // Play audio
        if (audioSource != null && openElevatorClip != null)
            audioSource.PlayOneShot(openElevatorClip);

        // Toggle visuals
        if (elevatorLight != null) elevatorLight.SetActive(isOpen);
        foreach (var btn in upOnButtons) if (btn != null) btn.SetActive(isOpen);
        foreach (var btn in upOffButtons) if (btn != null) btn.SetActive(!isOpen);
    }
}
