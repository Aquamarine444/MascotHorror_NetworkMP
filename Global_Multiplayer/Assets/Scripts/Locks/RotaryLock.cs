using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class RotaryLock : MonoBehaviour
{
    public GameObject dialObject;

    public int minimumDegrees = 36; // How much rotation counts as a "step" for each key press

    public int[] digitalAngles = new int[] { 36, -108, 216 }; // The angles that are to be set 
    public bool[] digitalRightTurn = new bool[] { true, false, true };
    public int[] digitalPasses = new int[] { 0, 0, 0 }; // How many times a player has to pass that number to set it

    private bool[] digitalSet;
    private int currentDigit = 0;

    private int passCount = 0;
    private bool passReset = false;
    private bool turningRight = true;
    private bool unlocked = false;

    private int oldAngle = 0;
    private int currentAngle = 0;

    private float rotationSpeed = 36f; // Rotation speed for each key press (in degrees)

    private bool jKeyPressed = false; // Flag to track if the J key was pressed
    private bool kKeyPressed = false; // Flag to track if the K key was pressed

    void Start()
    {
        digitalSet = new bool[digitalAngles.Length];
    }

    void Update()
    {
        if (unlocked)
        {
            // If unlocked, destroy the lock after a short delay or instantly
            DestroyLock();
            return;
        }

        // Check for key presses to turn the dial (only once per press)
        if (Keyboard.current.jKey.isPressed && !jKeyPressed) // Turn right (clockwise) on X (positive)
        {
            RotateDial(rotationSpeed);
            jKeyPressed = true; // Set flag to true so we don't keep rotating when the key is held down
        }
        if (Keyboard.current.kKey.isPressed && !kKeyPressed) // Turn left (counterclockwise) on X (negative)
        {
            RotateDial(-rotationSpeed);
            kKeyPressed = true; // Set flag to true so we don't keep rotating when the key is held down
        }

        // Reset flags when keys are released
        if (Keyboard.current.jKey.wasReleasedThisFrame)
        {
            jKeyPressed = false;
        }
        if (Keyboard.current.kKey.wasReleasedThisFrame)
        {
            kKeyPressed = false;
        }

        NormalizeDialRotation();
        UpdateCurrentAngle();
        UpdateTurningDirection();
        CheckPassReset();
        CheckDigitMatch();
        CheckUnlock();
    }

    private void RotateDial(float amount)
    {
        // Apply rotation to the dial object on the X axis, but keep Y and Z unchanged
        dialObject.transform.Rotate(amount, 0, 0);
    }

    private void NormalizeDialRotation()
    {
        // Normalize the dial rotation to ensure it stays within the 0-360 range on the X axis
        float xRotation = dialObject.transform.localEulerAngles.x;
        xRotation = Mathf.Repeat(xRotation, 360f); // Normalize angle
        dialObject.transform.localEulerAngles = new Vector3(xRotation, dialObject.transform.localEulerAngles.y, dialObject.transform.localEulerAngles.z);
    }

    private void UpdateCurrentAngle()
    {
        currentAngle = Mathf.RoundToInt(dialObject.transform.localEulerAngles.x / minimumDegrees) * minimumDegrees;
        Debug.Log($"Current Angle: {currentAngle}");  // Debug current angle
    }

    private void UpdateTurningDirection()
    {
        if (currentAngle % minimumDegrees == 0)
        {
            if (currentAngle < oldAngle && oldAngle != 0 && oldAngle != 360)
            {
                turningRight = false;
            }
            else if (currentAngle > oldAngle && oldAngle != 0 && oldAngle != 360)
            {
                turningRight = true;
            }
            oldAngle = currentAngle;
        }

        // Wrong direction resets everything
        if (digitalRightTurn[currentDigit] != turningRight && oldAngle != currentAngle)
        {
            Debug.Log("Unlock failed: wrong turn direction");
            ResetLockProgress();
        }
    }

    private void CheckPassReset()
    {
        if (!passReset && currentAngle != digitalAngles[currentDigit])
        {
            passReset = true;
            Debug.Log($"Pass reset for digit {currentDigit}");  // Debug pass reset
        }
    }

    private void CheckDigitMatch()
    {
        if (currentAngle == digitalAngles[currentDigit])
        {
            Debug.Log($"Angle matched: {currentAngle} == {digitalAngles[currentDigit]}"); // Debug if angle matches
            if (passReset)
            {
                passCount++;
                passReset = false;
                Debug.Log($"Pass Count for Digit {currentDigit}: {passCount}");  // Debug pass count
            }

            if (passCount >= digitalPasses[currentDigit])
            {
                Debug.Log($"Digit {currentDigit} set!");
                digitalSet[currentDigit] = true;

                if (currentDigit < digitalAngles.Length - 1)
                {
                    currentDigit++;
                    turningRight = digitalRightTurn[currentDigit];
                    passCount = 0;
                }
            }
        }
    }

    private void CheckUnlock()
    {
        bool allDigitsSet = true;
        for (int i = 0; i < digitalSet.Length; i++)
        {
            if (!digitalSet[i])
            {
                allDigitsSet = false;
                break;
            }
        }

        if (allDigitsSet)
        {
            unlocked = true;
            Debug.Log("Lock unlocked!");
            OnUnlock();
        }
    }

    private void ResetLockProgress()
    {
        passCount = 0;
        currentDigit = 0;
        passReset = false;
        turningRight = true;
        for (int i = 0; i < digitalSet.Length; i++)
        {
            digitalSet[i] = false;
        }
    }

    private void OnUnlock()
    {
        // Unlock logic (play sound, open door, etc.)
        Debug.Log("You can now open the door!");
    }

    private void DestroyLock()
    {
        // Destroy the lock after it's unlocked
        Destroy(gameObject);  // Remove the lock object from the scene
        Debug.Log("Lock destroyed.");
    }
}