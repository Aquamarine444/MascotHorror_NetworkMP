using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Interactors : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask interactLayer;

    [SerializeField] private IInteractible teractible;
    [SerializeField] private Transform teractibleTransform;
    [SerializeField] private Collider teractibleCollider;
    [SerializeField] private GameObject teractibleCamera;
    [SerializeField] private ObjectInspect OI;

    public GameObject interactUI;
    public GameObject minicrosshairUI;

    [SerializeField] private bool canRaycast = true;



    private void Update()
    {
        if (!isLocalPlayer) return; // ensure only local player runs this logic

        if (canRaycast)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity))
            {
                if ((interactLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    teractibleTransform = hit.transform;
                    teractibleCollider = hit.collider;
                    teractible = hit.collider.GetComponent<IInteractible>();

                    if (teractible != null)
                    {
                        teractibleCamera = teractible.ExamineCam;
                        OutlineOn();
                        EnableInteractUI();
                        TryInteract();
                    }
                }
                else
                {
                    OutlineOff();
                    DisableInteractUI();
                }
            }
            else
            {
                OutlineOff();
                DisableInteractUI();
            }
        }
    }

    private void TryInteract()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            DisableInteractUI();
            OutlineOff();
            minicrosshairUI.SetActive(false);

            if (teractible != null)
                teractible.Interact(this); // This should call a Command if needed internally

            // Conditional logic
            if (teractible != null)
            {
                if (teractible.Inspect && !teractible.Examine && !teractible.Trigger && !teractible.Place)
                    Inspect(teractibleTransform);

                else if (teractible.Examine && !teractible.Inspect && !teractible.Trigger && !teractible.Place)
                    Examine(teractibleCamera);

                else if (teractible.Trigger && !teractible.Inspect && !teractible.Examine && !teractible.Place)
                    Trigger();

                else if (teractible.Place && !teractible.Inspect && !teractible.Examine && !teractible.Trigger)
                    Place(teractibleCamera);
            }

            canRaycast = false;
        }
    }

    public void OutlineOn()
    {
        if (teractibleCollider != null)
        {
            var outline = teractibleCollider.GetComponent<Outline>();
            if (outline != null) outline.enabled = true;
        }
    }

    public void OutlineOff()
    {
        if (teractibleCollider != null)
        {
            var outline = teractibleCollider.GetComponent<Outline>();
            if (outline != null) outline.enabled = false;
        }
    }

    public void EnableInteractUI() => interactUI.SetActive(true);
    public void DisableInteractUI() => interactUI.SetActive(false);
    public void EnableRaycast() => canRaycast = true;

    private void Inspect(Transform T) => OI.Pickup(T);
    private void Examine(GameObject C) => OI.ZoomIn(C);
    private void Trigger() => OI.Trigger();
    private void Place(GameObject C) => OI.Place(C);

    private void OnDrawGizmos()
    {
        if (playerCamera == null) return;

        Vector3 cameraCenter = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, playerCamera.nearClipPlane));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cameraCenter, cameraCenter + playerCamera.transform.forward * 3f);

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity, interactLayer))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(playerCamera.transform.position, hit.point);
        }
    }
}
