using System.Collections;
using UnityEngine;
using Mirror;

public class FuseSwitchInteract : NetworkBehaviour, IInteractible
{
    [SerializeField] private bool inspect;
    [SerializeField] private bool examine;
    [SerializeField] private bool trigger;
    [SerializeField] private bool place;
    [SerializeField] private GameObject examineCamera;

    public bool Inspect => inspect;
    public bool Examine => examine;
    public bool Trigger => trigger;
    public bool Place => place;
    public GameObject ExamineCam => examineCamera;

    public string text;
    public CommsManager cM;

    [SyncVar(hook = nameof(OnFuseStateChanged))]
    public bool isOn;

    public GameObject light;
    public Material LightBlank;
    public Material lightRed;
    public Material lightGreen;

    public PowerManager power;

    private GameObject player;

    public GameObject elevator;
    public GameObject controlRoomDoor;
    public DoorManager dM;

    public bool Interact(Interactors interact)
    {
        player = interact.gameObject;

        // Send fuse toggle command to server with player's fuse state
        bool hasFuse = player.GetComponent<PlayerInventory>().fuseFilled;
        CmdToggleFuseSwitch(hasFuse);

        return false;
    }

    [Command(requiresAuthority = false)]
    void CmdToggleFuseSwitch(bool playerHasFuse)
    {
        isOn = !isOn;

        if (isOn)
        {
            power.fuseTrigger = true;

            if (!playerHasFuse)
            {
                RpcSetLight("red");
            }
            else
            {
                RpcSetLight("green");
                RpcOpenElevatorDoors(); //  Sync elevator & door state to all clients
                Debug.Log("Doors Open");
            }
        }
        else
        {
            power.fuseTrigger = false;
            RpcSetLight("blank");
        }

        RpcSetFuseAnim(isOn);
    }

    // Hook to change isOn visuals on clients
    void OnFuseStateChanged(bool oldValue, bool newValue)
    {
        RpcSetFuseAnim(newValue);
    }

    [ClientRpc]
    void RpcSetFuseAnim(bool on)
    {
        Animator anim = gameObject.transform.parent.GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("isOn", on);
    }

    [ClientRpc]
    void RpcSetLight(string color)
    {
        if (!light) return;

        var renderer = light.GetComponent<MeshRenderer>();
        if (renderer == null) return;

        switch (color)
        {
            case "red":
                renderer.material = lightRed;
                break;
            case "green":
                renderer.material = lightGreen;
                break;
            default:
                renderer.material = LightBlank;
                break;
        }
    }

    [ClientRpc]
    void RpcOpenElevatorDoors()
    {
        if (elevator != null)
            elevator.SetActive(false);

        if (controlRoomDoor != null)
            controlRoomDoor.SetActive(false);
    }

    [Server]
    public void ForceTurnOff()
    {
        isOn = false;
        power.fuseTrigger = false;
        RpcSetFuseAnim(false);
        RpcSetLight("blank");
    }
}
