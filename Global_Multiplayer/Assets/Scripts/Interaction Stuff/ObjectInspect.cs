using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ObjectInspect : NetworkBehaviour
{
    Camera mainCam;
    [SerializeField] private GameObject ExamineCam;
    [SerializeField] private GameObject zoomCam;
    [SerializeField] private Camera zoomCamera;
    GameObject clickedObject;

    Vector3 originaPosition;
    Vector3 originalRotation;

    public bool examineMode;
    public bool zoomMode;
    public bool TriggerMode;

    [SerializeField] private Volume postProcessVol;
    [SerializeField] private DepthOfField dOF;

    [SerializeField] private GameObject UIComments;
    [SerializeField] private GameObject UIPrompts;

    public float zoomSpeed = 2.0f;
    public GameObject Player;

    void Start()
    {
        mainCam = Camera.main;
        examineMode = false;
        zoomMode = false;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        TurnObject();
        ExitExamineMode();
    }

    public void Pickup(Transform objectTransform)
    {
        if (!isLocalPlayer) return;

        UIPrompts.SetActive(true);

        if (!examineMode)
        {
            ExamineCam.SetActive(true);

            clickedObject = objectTransform.gameObject;
            originaPosition = clickedObject.transform.position;
            originalRotation = clickedObject.transform.rotation.eulerAngles;

            clickedObject.transform.position = ExamineCam.transform.position + (transform.forward * 3f);

            Time.timeScale = 0;
            examineMode = true;
        }
    }

    public void ZoomIn(GameObject ZoomCam)
    {
        if (!isLocalPlayer) return;

        UIPrompts.SetActive(true);

        if (!zoomMode)
        {
            zoomCam = ZoomCam;
            zoomCamera = ZoomCam.GetComponent<Camera>();

            zoomCam.SetActive(true);
            gameObject.GetComponent<Camera>().enabled = false;

            Time.timeScale = 0;
            zoomMode = true;
        }
    }

    public void Trigger()
    {
        if (!isLocalPlayer) return;
        StartCoroutine(TriggerRoutine());
    }

    private IEnumerator TriggerRoutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        var interactors = Player.GetComponent<Interactors>();
        interactors.EnableRaycast();
        interactors.minicrosshairUI.SetActive(true);

        UIComments.SetActive(false);
        UIPrompts.SetActive(false);
    }

    public void Place(GameObject ZoomCam)
    {
        if (!isLocalPlayer) return;

        zoomCam = ZoomCam;
        zoomCamera = ZoomCam.GetComponent<Camera>();

        zoomCam.SetActive(true);
        gameObject.GetComponent<Camera>().enabled = false;

        StartCoroutine(PlaceRoutine());
    }

    private IEnumerator PlaceRoutine()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        var interactors = Player.GetComponent<Interactors>();
        interactors.EnableRaycast();
        interactors.minicrosshairUI.SetActive(true);

        UIComments.SetActive(false);
        UIPrompts.SetActive(false);

        zoomCam.SetActive(false);
        gameObject.GetComponent<Camera>().enabled = true;

        Player.GetComponent<Interactors>().EnableRaycast();
    }

    void TurnObject()
    {
        if (!isLocalPlayer || !examineMode) return;

        if (Input.GetMouseButton(0))
        {
            float rotationSpeed = 15f;
            float xAxis = Input.GetAxis("Mouse X") * rotationSpeed;
            float yAxis = Input.GetAxis("Mouse Y") * rotationSpeed;

            clickedObject.transform.Rotate(Vector3.up, -xAxis, Space.World);
            clickedObject.transform.Rotate(Vector3.right, yAxis, Space.World);
        }
    }

    public void ForceExit()
    {
        if (!isLocalPlayer) return;

        zoomCam.SetActive(false);
        gameObject.GetComponent<Camera>().enabled = true;

        Time.timeScale = 1;
        zoomMode = false;

        Player.GetComponent<Interactors>().EnableRaycast();
        Player.GetComponent<Interactors>().minicrosshairUI.SetActive(true);

        UIComments.SetActive(false);
        UIPrompts.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ExitExamineMode()
    {
        if (!isLocalPlayer) return;

        if (Input.GetMouseButtonDown(1))
        {
            if (examineMode)
            {
                ExamineCam.SetActive(false);

                clickedObject.transform.position = originaPosition;
                clickedObject.transform.eulerAngles = originalRotation;

                Time.timeScale = 1;
                examineMode = false;

                Player.GetComponent<Interactors>().EnableRaycast();
                Player.GetComponent<Interactors>().minicrosshairUI.SetActive(true);

                Destroy(clickedObject);  // NOTE: Only safe if it's a temporary/local clone
                UIComments.SetActive(false);
                UIPrompts.SetActive(false);
            }
            else if (zoomMode)
            {
                zoomCam.SetActive(false);
                gameObject.GetComponent<Camera>().enabled = true;

                Time.timeScale = 1;
                zoomMode = false;

                Player.GetComponent<Interactors>().EnableRaycast();
                Player.GetComponent<Interactors>().minicrosshairUI.SetActive(true);

                UIComments.SetActive(false);
                UIPrompts.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
