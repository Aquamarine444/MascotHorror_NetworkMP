using UnityEngine;

public class DeathScript : MonoBehaviour
{
    public bool Death;
    public GameObject DeathCanvas;
    public GameObject Player;

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
        DeathCanvas.SetActive(true);
        Player.SetActive(false);
    }
}
