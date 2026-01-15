using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public bool IsOpen = true;
    [SerializeField] private bool IsRotatingDoor = true;
    [SerializeField] private float Speed = 1f;
    
    [SerializeField] private bool _isNoteTaken = false; 
    private bool _isLockedByManager = false;

    [Header("RotationAmount")]
    [SerializeField] private float RotationAmount = 90f;
    [SerializeField] private float ForwardDirection = 0f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private AudioClip lockedSound;
    [SerializeField] private float LockedSoundCooldown = 1.5f; 
    private float _nextLockedSoundTime = 0f; 

    private Vector3 StartRotation;
    private Vector3 Forward;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        Forward = transform.right;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
    
    public void UnlockDoor()
    {
        _isNoteTaken = true;
    }

    public void SetLevelTransitionLock(bool state)
    {
        _isLockedByManager = state;
    }

    public void Open(Vector3 UserPosition)
    {
        if (this == null) return;

        if (_isLockedByManager || !_isNoteTaken) 
        {
            if (Time.time >= _nextLockedSoundTime)
            {
                if(lockedSound != null) PlaySound(lockedSound);
                _nextLockedSoundTime = Time.time + LockedSoundCooldown;
            }
            return; 
        }

        if(!IsOpen)
        {
            if(AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            PlaySound(doorSound);

            if(IsRotatingDoor)
            {
                float dot = Vector3.Dot(Forward, (UserPosition - transform.position).normalized);
                AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }
    
    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if(ForwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y - RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));
        }

        IsOpen = true;

        float time = 0;
        while(time < 1)
        {
            if (this == null) yield break;

            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    public void Close()
    {
        if (this == null) return;

        if(IsOpen)
        {
            if(AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);   
            }

            PlaySound(doorSound);

            if(IsRotatingDoor)
            {
                AnimationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);

        IsOpen = false;

        float time = 0;
        while(time < 1)
        {
            if (this == null) yield break;

            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (this == null || audioSource == null || clip == null) return;
     
        audioSource.PlayOneShot(clip);
    }
}