using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PowerMeter : NetworkBehaviour
{
    public Image[] powerBars; // Assign in inspector
    [SyncVar(hook = nameof(OnPowerChanged))]
    private int currentBarIndex = -1;

    private void Start()
    {
        if (isServer)
        {
            // Start with all bars disabled on server, sync to clients
            currentBarIndex = -1;
        }
        UpdateVisuals();
    }

    public void StartMeter()
    {
        if (isServer)
        {
            currentBarIndex = powerBars.Length - 1;
        }
    }

    public void RemoveBar()
    {
        if (!isServer) return; // Only server modifies syncvars

        if (currentBarIndex >= 0)
        {
            currentBarIndex--;
        }
    }

    public void ResetMeter()
    {
        if (!isServer) return;

        currentBarIndex = powerBars.Length - 1;
    }

    public void RemoveAll()
    {
        if (!isServer) return;

        currentBarIndex = -1;
    }

    void OnPowerChanged(int oldVal, int newVal)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (powerBars == null || powerBars.Length == 0)
            return;

        for (int i = 0; i < powerBars.Length; i++)
        {
            powerBars[i].enabled = (i <= currentBarIndex);
        }
    }
}
