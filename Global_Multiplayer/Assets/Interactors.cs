using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactors : MonoBehaviour
{

    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask interactLayer;

    private IInteractible teractible;

    private Transform teractibleTransform;
    private Collider teractibleCollider;
    //[SerializeField] private int teractiblesFound;
    [SerializeField] private GameObject teractibleCamera;

    public ObjectInspect OI;

    [SerializeField] public GameObject interactUI;
    public GameObject minicrosshairUI;


    [SerializeField] private bool canRaycast = true;


    private void Update()
    {
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
                        teractibleCamera = teractible.ExamineCam != null ? teractible.ExamineCam : teractibleCamera;

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
        if (Input.GetKeyDown(KeyCode.E))
        {

            DisableInteractUI();
            OutlineOff();

            minicrosshairUI.SetActive(false);

            // If you want to pick up the item
            if (teractible.Inspect && !teractible.Examine)
            {
                Inspect(teractibleTransform);
                //teractible.InventoryItem.SetActive(true);
            }

            // If you want to zoom in
            else if (!teractible.Inspect && teractible.Examine)
            {
                Examine(teractibleCamera);
            }


            canRaycast = false;
            teractible.Interact(this);



        }

    }

    public void OutlineOn()
    {
        if (teractibleCollider != null)
        {
            var outline = teractibleCollider.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = true;
            }
        }
    }

    public void OutlineOff()
    {
        if (teractibleCollider != null)
        {
            var outline = teractibleCollider.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    public void EnableInteractUI()
    {
        interactUI.SetActive(true);
    }

    public void DisableInteractUI()
    {
        interactUI.SetActive(false);
    }

    private void Inspect(Transform T)
    {
        OI.Pickup(T);
    }

    private void Examine(GameObject C)
    {
        OI.ZoomIn(C);
    }

    public void EnableRaycast()
    {
        canRaycast = true;
    }

    private void OnDrawGizmos()
    {
        // Align gizmo with the player's camera crosshair.
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
