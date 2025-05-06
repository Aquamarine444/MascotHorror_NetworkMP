using UnityEngine;

public class DeathScript : MonoBehaviour
{
    public bool Death;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death"))
        {
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        Death = true;

        gameObject.SetActive(false);
    }
}
