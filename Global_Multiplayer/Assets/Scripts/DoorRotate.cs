using UnityEngine;

public class DoorRotate : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 97f;
    public float closeAngle = 0f;
    public float animationDuration = 0.5f;   // seconds to complete swing

    [Header("Audio Settings")]
    public AudioClip openClip;
    public AudioClip closeClip;

    private AudioSource _audioSource;
    private bool _isOpen = false;
    private bool _isAnimating = false;
    private float _currentTime = 0f;
    private float _startAngle;
    private float _targetAngle;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        transform.localRotation = Quaternion.Euler(0f, closeAngle, 0f);
    }

    void Update()
    {
        if (_isAnimating)
        {
            _currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(_currentTime / animationDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            float currentY = Mathf.Lerp(_startAngle, _targetAngle, t);
            transform.localRotation = Quaternion.Euler(0f, currentY, 0f);

            if (_currentTime >= animationDuration)
            {
                _isAnimating = false;
                // Ensure exact final angle
                transform.localRotation = Quaternion.Euler(0f, _targetAngle, 0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isOpen && !_isAnimating)
            BeginAnimation(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _isOpen && !_isAnimating)
            BeginAnimation(false);
    }

    private void BeginAnimation(bool open)
    {
        _isOpen = open;
        PlaySound(open);

        // Setup animation angles explicitly
        _startAngle = open ? closeAngle : openAngle;
        _targetAngle = open ? openAngle : closeAngle;
        _currentTime = 0f;
        _isAnimating = true;
    }

    private void PlaySound(bool opening)
    {
        AudioClip clip = opening ? openClip : closeClip;
        if (clip == null) return;
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}