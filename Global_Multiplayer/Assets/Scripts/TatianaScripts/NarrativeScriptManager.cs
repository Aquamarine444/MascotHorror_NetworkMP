using Mirror;
using UnityEngine;

public class NarrativeScriptManager : NetworkBehaviour
{
    [Header("UI GameObjects")]
    public GameObject Poster;
    public GameObject Article;
    public GameObject Narrative;
    public GameObject Text_001;
    public GameObject Text_002;
    public GameObject Controls;
    public GameObject Button;
    public GameObject Audio;
    public GameObject SecondAudio;
    public GameObject MommaNarrative;
    public GameObject MommaLoad;

    [Header("Timer Settings")]
    public float Timer = 5f;
    public float Timer2 = 3f;
    public float Timer3 = 4f;
    public float Timer4 = 2f;
    public float Timer5 = 1f;

    // SyncVars to keep timers synchronized across network
    [SyncVar] private float syncTimer;
    [SyncVar] private float syncTimer2;
    [SyncVar] private float syncTimer3;
    [SyncVar] private float syncTimer4;
    [SyncVar] private float syncTimer5;

    // SyncVar to track current narrative state
    [SyncVar(hook = nameof(OnNarrativeStateChanged))]
    private NarrativeState currentState = NarrativeState.Initial;

    // SyncVar for audio state
    [SyncVar(hook = nameof(OnAudioStateChanged))]
    private bool isFirstAudioActive = false;

    public enum NarrativeState
    {
        Initial,
        ShowingPoster,
        ShowingArticle,
        ShowingNarrative,
        ShowingText002,
        ShowingControls,
        ShowingButton,
        MommaSequence
    }

    public override void OnStartServer()
    {
        // Initialize timers on server
        syncTimer = Timer;
        syncTimer2 = Timer2;
        syncTimer3 = Timer3;
        syncTimer4 = Timer4;
        syncTimer5 = Timer5;

        // Set initial state
        currentState = NarrativeState.ShowingPoster;
        isFirstAudioActive = false;
    }

    private void Start()
    {
        // Initial setup - this will be overridden by SyncVar hooks
        if (isClient)
        {
            UpdateUIState();
            UpdateAudioState();
        }
    }

    private void Update()
    {
        // Only server handles timer logic
        if (!isServer) return;

        HandleTimerLogic();
        HandleInput();
    }

    private void HandleTimerLogic()
    {
        switch (currentState)
        {
            case NarrativeState.ShowingPoster:
                if (syncTimer > 0)
                {
                    syncTimer -= Time.deltaTime;
                }
                else
                {
                    currentState = NarrativeState.ShowingArticle;
                }
                break;

            case NarrativeState.ShowingArticle:
                if (syncTimer2 > 0)
                {
                    syncTimer2 -= Time.deltaTime;
                }
                else
                {
                    currentState = NarrativeState.ShowingNarrative;
                }
                break;

            case NarrativeState.ShowingNarrative:
                if (syncTimer3 > 0)
                {
                    syncTimer3 -= Time.deltaTime;
                }
                else
                {
                    currentState = NarrativeState.ShowingText002;
                }
                break;

            case NarrativeState.ShowingText002:
                if (syncTimer4 > 0)
                {
                    syncTimer4 -= Time.deltaTime;
                }
                else
                {
                    currentState = NarrativeState.ShowingControls;
                }
                break;

            case NarrativeState.ShowingControls:
                if (syncTimer5 > 0)
                {
                    syncTimer5 -= Time.deltaTime;
                }
                else
                {
                    currentState = NarrativeState.ShowingButton;
                }
                break;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TriggerMommaSequence();
        }
    }

    [Server]
    private void TriggerMommaSequence()
    {
        currentState = NarrativeState.MommaSequence;
        isFirstAudioActive = true;
    }

    // Network synchronized method for the Continue button
    [Command(requiresAuthority = false)]
    public void CmdContinue()
    {
        if (!isServer) return;

        isFirstAudioActive = true;
        // You might want to set a specific state here or trigger other logic
        // For now, just activating MommaLoad
        RpcActivateMommaLoad();
    }

    [ClientRpc]
    private void RpcActivateMommaLoad()
    {
        if (MommaLoad != null)
            MommaLoad.SetActive(true);
    }

    // Public method to be called by UI Button
    public void Continue()
    {
        CmdContinue();
    }

    // SyncVar hook for state changes
    private void OnNarrativeStateChanged(NarrativeState oldState, NarrativeState newState)
    {
        UpdateUIState();
    }

    // SyncVar hook for audio changes
    private void OnAudioStateChanged(bool oldValue, bool newValue)
    {
        UpdateAudioState();
    }

    private void UpdateUIState()
    {
        // Reset all UI elements
        SetAllUIInactive();

        // Activate appropriate UI based on current state
        switch (currentState)
        {
            case NarrativeState.ShowingPoster:
                if (Poster != null) Poster.SetActive(true);
                break;

            case NarrativeState.ShowingArticle:
                if (Article != null) Article.SetActive(true);
                break;

            case NarrativeState.ShowingNarrative:
                if (Narrative != null) Narrative.SetActive(true);
                if (Text_001 != null) Text_001.SetActive(true);
                break;

            case NarrativeState.ShowingText002:
                if (Narrative != null) Narrative.SetActive(true);
                if (Text_001 != null) Text_001.SetActive(false);
                if (Text_002 != null) Text_002.SetActive(true);
                break;

            case NarrativeState.ShowingControls:
                if (Controls != null) Controls.SetActive(true);
                break;

            case NarrativeState.ShowingButton:
                if (Controls != null) Controls.SetActive(true);
                if (Button != null) Button.SetActive(true);
                break;

            case NarrativeState.MommaSequence:
                if (Poster != null) Poster.SetActive(true);
                if (Controls != null) Controls.SetActive(false);
                if (MommaNarrative != null) MommaNarrative.SetActive(false);
                if (MommaLoad != null) MommaLoad.SetActive(true);
                break;
        }
    }

    private void UpdateAudioState()
    {
        if (Audio != null) Audio.SetActive(isFirstAudioActive);
        if (SecondAudio != null) SecondAudio.SetActive(!isFirstAudioActive);
    }

    private void SetAllUIInactive()
    {
        if (Poster != null) Poster.SetActive(false);
        if (Article != null) Article.SetActive(false);
        if (Narrative != null) Narrative.SetActive(false);
        if (Text_001 != null) Text_001.SetActive(false);
        if (Text_002 != null) Text_002.SetActive(false);
        if (Controls != null) Controls.SetActive(false);
        if (Button != null) Button.SetActive(false);
        //if (MommaNarrative != null) MommaNarrative.SetActive(false);
        if (MommaLoad != null) MommaLoad.SetActive(false);
    }
}