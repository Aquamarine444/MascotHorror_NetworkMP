using UnityEngine;
using Mirror;

public class Flashlight : NetworkBehaviour
{
    public GameObject Light;

    [SyncVar(hook = nameof(OnFlashlightStateChanged))]
    public bool isFlashlightOn = false;

    private void Update()
    {
        // Only allow the owner to control the flashlight
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            // Call command to toggle flashlight on server
            CmdToggleFlashlight();
        }
    }

    [Command]
    void CmdToggleFlashlight()
    {
        // Toggle the state on the server
        isFlashlightOn = !isFlashlightOn;
    }

    // This method is called whenever the SyncVar changes
    void OnFlashlightStateChanged(bool oldValue, bool newValue)
    {
        Light.SetActive(newValue);
    }

    // Initialize the light state when the object starts
    public override void OnStartClient()
    {
        Light.SetActive(isFlashlightOn);
    }
}
