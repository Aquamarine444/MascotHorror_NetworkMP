using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class RotaryLock : MonoBehaviour
{
    [Header("Lock Settings")]
    [Tooltip("Configure your combo angles here (multiples of stepAngle)")]
    public List<float> targetAngles = new List<float> { 72f, 288f, 108f };   // now an Inspector-editable List

    [Tooltip("How many degrees each click moves (must divide 360)")]
    public float stepAngle = 36f;

    [Tooltip("± how close to count as 'hit'")]
    public float angleTolerance = 5f;

    private int currentTargetIndex = 0;
    private bool mustReturnToZero = false;
    private bool isUnlocked = false;
    private float currentAngle = 0f;


    public GameObject lockerDoor;

    void Start()
    {
        currentAngle = NormalizeAngle(transform.eulerAngles.x);
        ApplyRotation();
        PrintNextTargetInstruction();
    }

    void Update()
    {
        if (isUnlocked) return;

        if (Input.GetKeyDown(KeyCode.J)) HandleTurn(-1);
        else if (Input.GetKeyDown(KeyCode.K)) HandleTurn(1);
    }

    private void HandleTurn(int direction)
    {
        // enforce correct direction
        if (!mustReturnToZero)
        {
            if (direction != GetTurnDirection(currentTargetIndex))
            {
                Debug.Log("❌ Wrong direction for target – resetting lock.");
                InstantReset();
                return;
            }
        }
        else if (currentTargetIndex < targetAngles.Count - 1)
        {
            if (direction != -GetTurnDirection(currentTargetIndex))
            {
                Debug.Log("❌ Wrong direction returning to zero – resetting lock.");
                InstantReset();
                return;
            }
        }

        // perform the click
        currentAngle = NormalizeAngle(currentAngle + stepAngle * direction);
        ApplyRotation();
        Debug.Log(direction == 1 ? "▶️ Clicked Clockwise" : "◀️ Clicked Counter-Clockwise");

        // check hit or return
        float angle = currentAngle;
        if (!mustReturnToZero)
        {
            float target = targetAngles[currentTargetIndex];
            if (Mathf.Abs(angle - target) <= angleTolerance)
            {
                if (currentTargetIndex == targetAngles.Count - 1)
                {
                    isUnlocked = true;
                    Debug.Log("🏆 Lock Fully Opened!");

                    lockerDoor.SetActive(false);

                    GetComponent<MissingDialInteract>().currentPlayer.GetComponentInChildren<ObjectInspect>().ForceExit();

                }
                else
                {
                    mustReturnToZero = true;
                    Debug.Log($"✅ Hit Target {currentTargetIndex}: {target}°. Now return to 0°.");
                    PrintReturnInstruction();
                }
            }
        }
        else
        {
            if (Mathf.Abs(angle) <= angleTolerance)
            {
                Debug.Log("✅ Returned to 0°!");
                mustReturnToZero = false;
                currentTargetIndex++;
                PrintNextTargetInstruction();
            }
        }
    }

    private void InstantReset()
    {
        currentAngle = 0f;
        ApplyRotation();
        currentTargetIndex = 0;
        mustReturnToZero = false;
        isUnlocked = false;
        Debug.Log("🔄 Lock Reset Instantly.");
        PrintNextTargetInstruction();
    }

    private int GetTurnDirection(int idx)
    {
        return (idx % 2 == 0) ? 1 : -1;
    }

    private void PrintNextTargetInstruction()
    {
        if (currentTargetIndex >= targetAngles.Count) return;
        int dir = GetTurnDirection(currentTargetIndex);
        string dirStr = dir == 1 ? "Clockwise (K)" : "Counter-Clockwise (J)";
        Debug.Log($"➡️ Turn {dirStr} to {targetAngles[currentTargetIndex]}°");
    }

    private void PrintReturnInstruction()
    {
        int ret = -GetTurnDirection(currentTargetIndex);
        string retStr = ret == 1 ? "Clockwise (K)" : "Counter-Clockwise (J)";
        Debug.Log($"⬅️ Return {retStr} to 0°");
    }

    private void ApplyRotation()
    {
        transform.rotation = Quaternion.Euler(currentAngle, 0f, 0f);
    }

    private float NormalizeAngle(float a)
    {
        a %= 360f;
        if (a < 0f) a += 360f;
        return a;
    }
}