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

    public override void OnStartServer()
    {
        leftClosedPos = leftDoor.localPosition;
        rightClosedPos = rightDoor.localPosition;
        leftOpenPos = leftClosedPos + Vector3.down * doorOffset;
        rightOpenPos = rightClosedPos + Vector3.up * doorOffset;
        // initial visuals on all clients
        RpcUpdateElevatorVisuals(false);
        RpcOpenElevator();
    }

    [ClientRpc]
    void RpcUpdateElevatorVisuals(bool open)
    {
        elevatorLight?.SetActive(open);
        foreach (var btn in upOnButtons) if (btn) btn.SetActive(open);
        foreach (var btn in upOffButtons) if (btn) btn.SetActive(!open);
    }

    [ClientRpc]
    void RpcOpenElevator()
    {
        isOpen = true;
        isMoving = true;
        if (audioSource && openElevatorClip)
            audioSource.PlayOneShot(openElevatorClip);
    }

    void Update()
    {
        if (!isServer && !isClient) return; // local animate
        if (isMoving)
        {
            Vector3 tL = isOpen ? leftOpenPos : leftClosedPos;
            Vector3 tR = isOpen ? rightOpenPos : rightClosedPos;
            leftDoor.localPosition = Vector3.MoveTowards(leftDoor.localPosition, tL, doorMoveSpeed * Time.deltaTime);
            rightDoor.localPosition = Vector3.MoveTowards(rightDoor.localPosition, tR, doorMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(leftDoor.localPosition, tL) < 0.001f &&
                Vector3.Distance(rightDoor.localPosition, tR) < 0.001f)
            {
                isMoving = false;
            }
        }
    }
}
