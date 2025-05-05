using UnityEngine;

public class DeathScript : MonoBehaviour
{
    private SyncedScreen Dead;

    private void Start()
    {
        Dead = GetComponent<SyncedScreen>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death"))
        {
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        Dead.Death = true;
        gameObject.SetActive(false);
    }
}
