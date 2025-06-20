using Mirror;
using UnityEngine;

public class NarrativeManagerSpawner : NetworkBehaviour
{
    [Header("Prefab Reference")]
    public GameObject narrativeManagerPrefab;

    private static NarrativeScriptManager spawnedManager;

    public override void OnStartServer()
    {
        if (narrativeManagerPrefab != null && spawnedManager == null)
        {
            GameObject spawned = Instantiate(narrativeManagerPrefab);
            NetworkServer.Spawn(spawned);
            spawnedManager = spawned.GetComponent<NarrativeScriptManager>();
        }
    }

    public override void OnStopServer()
    {
        if (spawnedManager != null)
        {
            NetworkServer.UnSpawn(spawnedManager.gameObject);
            Destroy(spawnedManager.gameObject);
            spawnedManager = null;
        }
    }
}

