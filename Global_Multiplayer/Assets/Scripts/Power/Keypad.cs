using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Keypad : MonoBehaviour
{
    public Doors targetDoor;
    public float delayBeforeDisable = 1.5f;
    public float delayBeforeReenable = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animatronic") && targetDoor != null)
        {
            targetDoor.ForceDisableAndEnable(delayBeforeDisable, delayBeforeReenable);
        }
    }
}
