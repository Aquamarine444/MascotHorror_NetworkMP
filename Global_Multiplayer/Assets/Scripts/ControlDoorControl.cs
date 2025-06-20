using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class ControlDoorControl : MonoBehaviour
{
    public Collider openZone;
    public Collider endZone;
    public float openHeight = 2f;
    public float openSpeed = 1f;
    public float dropSpeed = 2f;
    public float snapThreshold = 0.01f;
    public Behaviour outlineComponent;
    public TMP_Text zoneText;
    public AudioClip openClip;
    public AudioClip closeClip;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool lockedClosed;
    private bool hasInteracted;
    private AudioSource audioSrc;
    private bool playedCloseOnRest;
    private bool wasMoving;

    void Start()
    {
        closedPos = transform.localPosition;
        openPos = closedPos + Vector3.up * openHeight;
        openZone.isTrigger = true;
        endZone.isTrigger = true;
        audioSrc = GetComponent<AudioSource>();
        audioSrc.loop = true;
        audioSrc.playOnAwake = false;
        audioSrc.clip = openClip;
    }

    void Update()
    {
        if (lockedClosed) return;
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;
        bool inOpenZone = openZone.bounds.Contains(player.transform.position);
        if (outlineComponent != null) outlineComponent.enabled = inOpenZone;
        if (zoneText != null) zoneText.enabled = inOpenZone;
        if (inOpenZone && Input.GetKeyDown(KeyCode.E))
            hasInteracted = true;
        if (endZone.bounds.Contains(player.transform.position))
        {
            LockAndClose();
            return;
        }
        bool shouldOpen = inOpenZone && Input.GetKey(KeyCode.E);
        Vector3 target = shouldOpen ? openPos : closedPos;
        float speed = shouldOpen ? openSpeed : dropSpeed;
        float dist = Vector3.Distance(transform.localPosition, target);
        bool isMoving = dist > snapThreshold;
        if (shouldOpen && isMoving)
        {
            if (!audioSrc.isPlaying) audioSrc.Play();
        }
        else if (audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
        Vector3 newPos = isMoving
            ? Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime)
            : target;
        transform.localPosition = newPos;
        if (!isMoving && wasMoving && !shouldOpen && hasInteracted)
        {
            audioSrc.PlayOneShot(closeClip);
        }
        wasMoving = isMoving;
    }

    private void LockAndClose()
    {
        lockedClosed = true;
        transform.localPosition = closedPos;
        if (audioSrc.isPlaying) audioSrc.Stop();
        if (hasInteracted)
            audioSrc.PlayOneShot(closeClip);
    }
}