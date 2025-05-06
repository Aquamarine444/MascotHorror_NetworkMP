using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Keypad : MonoBehaviour
{
    public GameObject door;
    public float disableDelay = 1.5f;
    public float reenableDelay = 2f;

    private bool isDisabled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDisabled) return;

        if (other.CompareTag("Animatronic"))
        {
            StartCoroutine(DisableAndReenable());
        }
    }

    private IEnumerator DisableAndReenable()
    {
        isDisabled = true;

        yield return new WaitForSeconds(disableDelay);
        door.SetActive(false);
        Debug.Log("Door disabled.");

        yield return new WaitForSeconds(reenableDelay);
        door.SetActive(true);
        Debug.Log("Door re-enabled.");

        isDisabled = false;
    }
}
