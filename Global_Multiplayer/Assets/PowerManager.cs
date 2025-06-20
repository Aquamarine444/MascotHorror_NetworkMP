using Mirror;
using UnityEngine;

public class PowerManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnPowerStateChanged))]
    public bool powerOn = false;

    public GameObject[] lights;

    [Header("Power Settings")]
    [SyncVar] public float powerLevel = 100f;
    [SyncVar] public float currentPower = 100f;
    public float powerDecreaseRate;

    [SyncVar(hook = nameof(OnFuseTriggerChanged))]
    public bool fuseTrigger = false;

    private float totalPowerLost = 0f;

    public FuseSwitchInteract fsI;

    [SyncVar] public int doorsOpen;

    public override void OnStartClient()
    {
        base.OnStartClient();
        SetLights(powerOn);
    }

    void Update()
    {
        if (!isServer) return; // Only the server should control power logic

        if (doorsOpen >= 3)
        {
           // powerOn = false;
        }

        if (powerOn)
        {
            float powerLostThisFrame = powerDecreaseRate * Time.deltaTime;
            currentPower -= powerLostThisFrame;
            totalPowerLost += powerLostThisFrame;

            while (totalPowerLost >= 10f)
            {
                totalPowerLost -= 10f;
                GetComponent<PowerMeter>()?.RemoveBar();
            }

            if (currentPower <= 0f)
            {
                currentPower = 0f;
                powerOn = false;
                Debug.Log("Power depleted. Turning off.");
            }
        }
    }

    [Server]
    public void SpendPower(float amount)
    {
        currentPower -= amount;
        totalPowerLost += amount;

        while (totalPowerLost >= 10f)
        {
            totalPowerLost -= 10f;
            GetComponent<PowerMeter>()?.RemoveBar();
        }

        if (currentPower <= 0f)
        {
            currentPower = 0f;
            powerOn = false;
            currentPower = powerLevel;
            Debug.Log("Power depleted due to door usage.");
        }
    }

    // Called by clients when they toggle fuse
    [Command(requiresAuthority = false)]
    public void CmdSetFuseTrigger(bool state)
    {
        fuseTrigger = state;
    }

    void OnPowerStateChanged(bool oldValue, bool newValue)
    {
        SetLights(newValue);
    }

    void OnFuseTriggerChanged(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            if (currentPower > 0f)
            {
                powerOn = true;

                Debug.Log("Power turned ON from fuse.");
                PowerMeter meter = GetComponent<PowerMeter>();
                if (meter != null)
                {
                    meter.ResetMeter();
                    meter.StartMeter();
                }
            }
        }
        else
        {
            powerOn = false;
            Debug.Log("Power turned OFF from fuse reset.");
        }
    }

    void SetLights(bool state)
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(state);
        }
    }
}
