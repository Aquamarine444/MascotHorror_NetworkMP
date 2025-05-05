using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ObjectInspect : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] private GameObject ExamineCam;
    [SerializeField] private GameObject zoomCam;
    [SerializeField] private Camera zoomCamera;
    GameObject clickedObject;

    Vector3 originaPosition;
    Vector3 originalRotation;

    [SerializeField] public bool examineMode;
    [SerializeField] public bool zoomMode;
    [SerializeField] public bool TriggerMode;


    [SerializeField] private Volume postProcessVol;
    [SerializeField] private DepthOfField dOF;

    [SerializeField] private GameObject UIComments;
    [SerializeField] private GameObject UIPrompts;

    public float zoomSpeed = 2.0f;

    public GameObject Player;

    void Start()
    {
        postProcessVol = GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<Volume>();
        postProcessVol.profile.TryGet<DepthOfField>(out dOF);

        mainCam = Camera.main;
        examineMode = false;
        zoomMode = false;
    }

    private void Update()
    {
        TurnObject();
        ExitExamineMode();


    }

    public void Pickup(Transform objectTransform)
    {
        UIPrompts.SetActive(true);

        if (examineMode == false)
        {
            ExamineCam.SetActive(true);

            clickedObject = objectTransform.gameObject;

            originaPosition = clickedObject.transform.position;
            originalRotation = clickedObject.transform.rotation.eulerAngles;

            clickedObject.transform.position = ExamineCam.transform.position + (transform.forward * 3f);

            Time.timeScale = 0;
            //dOF.active = true;
            postProcessVol.profile.TryGet<DepthOfField>(out dOF);
            dOF.mode.value = DepthOfFieldMode.Bokeh;

            examineMode = true;
        }

    }

    public void ZoomIn(GameObject ZoomCam)
    {
        UIPrompts.SetActive(true);

        if (zoomMode == false)
        {
            zoomCam = ZoomCam;

            zoomCamera = ZoomCam.GetComponent<Camera>();

            zoomCam.SetActive(true);
            gameObject.GetComponent<Camera>().enabled = false;


            Time.timeScale = 0;
            //dOF.active = true;
            postProcessVol.profile.TryGet<DepthOfField>(out dOF);
            //dOF.mode.value = DepthOfFieldMode.Bokeh;

            zoomMode = true;
        }

    }

    public void Trigger()
    {
        StartCoroutine(TriggerRoutine());
    }

    private IEnumerator TriggerRoutine()
    {
        yield return new WaitForSeconds(.5f);

        var interactors = Player.GetComponent<Interactors>();
        interactors.EnableRaycast();
        interactors.minicrosshairUI.SetActive(true);

        UIComments.SetActive(false);
        UIPrompts.SetActive(false);
    }


    public void Place(GameObject ZoomCam)
    {
            zoomCam = ZoomCam;
            zoomCamera = ZoomCam.GetComponent<Camera>();

            zoomCam.SetActive(true);
            gameObject.GetComponent<Camera>().enabled = false;


            //Time.timeScale = 0;
            //dOF.active = true;
            postProcessVol.profile.TryGet<DepthOfField>(out dOF);
            dOF.mode.value = DepthOfFieldMode.Bokeh;

            StartCoroutine(PlaceRoutine());
    }

    private IEnumerator PlaceRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        var interactors = Player.GetComponent<Interactors>();
        interactors.EnableRaycast();
        interactors.minicrosshairUI.SetActive(true);

        UIComments.SetActive(false);
        UIPrompts.SetActive(false);


        zoomCam.SetActive(false);
        gameObject.GetComponent<Camera>().enabled = true;

        postProcessVol.profile.TryGet<DepthOfField>(out dOF);
        dOF.mode.value = DepthOfFieldMode.Off;


        Player.GetComponent<Interactors>().EnableRaycast();


    }

    void TurnObject()
    {
        if (Input.GetMouseButton(0) && examineMode)
        {

            float rotationSpeed = 15;

            float xAxis = Input.GetAxis("Mouse X") * rotationSpeed;
            float yAxis = Input.GetAxis("Mouse Y") * rotationSpeed;

            clickedObject.transform.Rotate(Vector3.up, -xAxis, Space.World);
            clickedObject.transform.Rotate(Vector3.right, yAxis, Space.World);
        }
    }

    void ExitExamineMode()
    {


        if (Input.GetMouseButtonDown(1) && examineMode)
        {

            ExamineCam.SetActive(false);

            clickedObject.transform.position = originaPosition;
            clickedObject.transform.eulerAngles = originalRotation;

            Time.timeScale = 1;
            //dOF.active = false;
            postProcessVol.profile.TryGet<DepthOfField>(out dOF);
            dOF.mode.value = DepthOfFieldMode.Off;

            examineMode = false;

            Player.GetComponent<Interactors>().EnableRaycast();

            Player.GetComponent<Interactors>().minicrosshairUI.SetActive(true);

            Destroy(clickedObject);

            UIComments.SetActive(false);
            UIPrompts.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1) && zoomMode)
        {

            zoomCam.SetActive(false);
            gameObject.GetComponent<Camera>().enabled = true;

            Time.timeScale = 1;
            //dOF.active = false;
            postProcessVol.profile.TryGet<DepthOfField>(out dOF);
            dOF.mode.value = DepthOfFieldMode.Off;

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

