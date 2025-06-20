using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Mirror;


public class ControlDoorControl : NetworkBehaviour
{
    public Collider openZone;
    public Collider endZone;
    public float openHeight = 2f;
    public float openSpeed = 1f;
    public float dropSpeed = 2f;
    public float snapThreshold = 0.01f;
    public Outline outlineComponent;
    public TMP_Text zoneText;
    public AudioClip openClip;
    public AudioClip closeClip;

    [SyncVar] private bool lockedClosed = false;
    [SyncVar] private bool hasInteracted = false;
    private Vector3 closedPos;
    private Vector3 openPos;
    private AudioSource audioSrc;
    private bool wasMoving = false;

    public override void OnStartServer()
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
        if (!isLocalPlayer) return;
        if (lockedClosed) return;

        // detect zones & input locally
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;
        bool inOpenZone = openZone.bounds.Contains(player.transform.position);

        outlineComponent.enabled = inOpenZone;
        if (zoneText) zoneText.enabled = inOpenZone;

        if (inOpenZone && Input.GetKeyDown(KeyCode.E))
        {
            hasInteracted = true;
            CmdRequestToggleDoor();
        }
    }

    [Command]
    void CmdRequestToggleDoor()
    {
        RpcToggleDoor();
    }

    [ClientRpc]
    void RpcToggleDoor()
    {
        // server-driven open/close logic on all clients
        Vector3 target;
        float speed;
        bool shouldOpen = hasInteracted;
        target = shouldOpen ? openPos : closedPos;
        speed = shouldOpen ? openSpeed : dropSpeed;

        bool isMoving = Vector3.Distance(transform.localPosition, target) > snapThreshold;

        // audio
        if (shouldOpen && isMoving)
        {
            if (!audioSrc.isPlaying) audioSrc.Play();
        }
        else if (audioSrc.isPlaying)
        {
            audioSrc.Stop();
            if (!shouldOpen && isMoving)
                audioSrc.PlayOneShot(closeClip);
        }

        // movement frame-by-frame via coroutine
        StopAllCoroutines();
        StartCoroutine(MoveDoor(target, speed));
    }

    System.Collections.IEnumerator MoveDoor(Vector3 target, float speed)
    {
        bool moving = true;
        while (moving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
            moving = Vector3.Distance(transform.localPosition, target) > snapThreshold;
            yield return null;
        }
        transform.localPosition = target;
        wasMoving = false;
    }
}