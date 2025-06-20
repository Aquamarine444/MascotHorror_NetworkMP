using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))] //automatically adds CharacterController to object that has this script
public class FPSPlayer : NetworkBehaviour //NetworkBehaviour - class that comes/inherited from Mirror
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("References")]
    public Transform playerCamera;

    [Header("Private Stuff")]
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalVelocity;
    private float cameraPitch = 0f;

    [Header("End Game")]
    public bool Dead = false;
    public bool Win = false;
    public string NextScene;

    public GameObject EndSound;

    public int Counter = 0;

    [Header("AnimationStuff")]
    public Animator AnimState;
    public GameObject PlayerRig;
    private bool isMoving = false;

    [SyncVar(hook = nameof(OnAnimationStateChanged))]
    private bool isWalking = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (!isLocalPlayer) //if not this computer's player, disable camera
        {
            playerCamera.gameObject.SetActive(false);
            gameObject.GetComponent<PlayerInput>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<PlayerInput>().enabled = true;
        }
    
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PlayerRig != null)
        {
            AnimState = PlayerRig.GetComponent<Animator>();
        }

        if (AnimState != null)
        {
            AnimState.SetBool("AnimFloat", true);
            AnimState.SetBool("AnimWalk", false);
        }

    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        UnityEngine.InputSystem.PlayerInput playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        playerInput.enabled = true;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Make sure we have the animator reference
        if (AnimState == null && PlayerRig != null)
        {
            AnimState = PlayerRig.GetComponent<Animator>();
        }

        // Apply current walking state
        if (AnimState != null)
        {
            AnimState.SetBool("AnimWalk", isWalking);
            AnimState.SetBool("AnimFloat", !isWalking);
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return; // return stops the code

        HandleMovement();
        HandleLook();

        MouseController();

        

    }

    private void HandleMovement()
    {
        if (!isLocalPlayer) return;

        // Translate move input to world space
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= moveSpeed;

        //checks if player is moving
        isMoving = move.x != 0 || move.y != 0;
        
        /*if (isMoving)
        {
            AnimState.SetBool("AnimWalk", true);
            AnimState.SetBool("AnimFloat", false);
        }

        if (!isMoving)
        {
            AnimState.SetBool("AnimWalk", false);
            AnimState.SetBool("AnimFloat", true);
        }*/

        //reduce network traffic
        if (isWalking != isMoving)
        {
            isWalking = isMoving;

            CmdUpdateAnimation(isMoving);
        }

       
        // Apply gravity
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }

    private void HandleLook()
    {
        if (!isLocalPlayer) return;

        float mouseX = lookInput.x * lookSpeed * Time.deltaTime;
        float mouseY = lookInput.y * lookSpeed * Time.deltaTime;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }


    public void OnMove(InputValue value) //connected to InputSystem ActionMap - get inputvalue to be able to use it in code(case sensitive)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value) //connected to InputSystem ActionMap
    {
        lookInput = value.Get<Vector2>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death"))
        {
            EndSound.SetActive(true);
            if (NetworkServer.active)
            {
                //loadLevel.allowSceneActivation = true;
                Debug.Log("Death");
                // Change scene for everyone
                NetworkManager.singleton.ServerChangeScene(NextScene);
            }
        }
    }

    public void MouseController()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Counter += 1;

            if (Counter % 2 == 1)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Counter % 2 == 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

        }
    }

    [Command]
    void CmdUpdateAnimation(bool walking)
    {
        RpcUpdateAnimation(walking);
    }

    [ClientRpc]
    void RpcUpdateAnimation(bool walking)
    {
        if (AnimState != null)
        {
            AnimState.SetBool("AnimWalk", walking);
            AnimState.SetBool("AnimFloat", !walking);
        }
    }

    // Hook method called when SyncVar changes
    private void OnAnimationStateChanged(bool oldValue, bool newValue)
    {

        // Ensure we have the animator reference
        if (AnimState == null && PlayerRig != null)
        {
            AnimState = PlayerRig.GetComponent<Animator>();
        }

        // This runs on all clients when the SyncVar changes
        if (AnimState != null)
        {
            AnimState.SetBool("AnimWalk", newValue);
            AnimState.SetBool("AnimFloat", !newValue);
        }

    }

    // This runs on all clients when the SyncVar changes
    /*AnimState.SetBool("AnimWalk", newValue);
    AnimState.SetBool("AnimFloat", !newValue);*/

}
