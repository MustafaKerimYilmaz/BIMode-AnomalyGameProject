using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StickyNoteController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject stickyNoteOnTable;
    [SerializeField] private TextMeshPro stickyNoteOnTableText;
    [SerializeField] private GameObject stickyNoteOnCamera;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect;
    
    [Header("Door Unlock Settings")]
    [SerializeField] private Door doorToUnlock;

    [Header("Text Settings")]
    [SerializeField] private GameObject textOnBlackboard;

    [Header("Post Processing")]
    [SerializeField] private Volume globalVolume;
    private DepthOfField _dof;

    [Header("Settings")]
    [SerializeField] private float actionCooldown = 0.5f;
    [SerializeField] private float transitionSpeed = 15f;
    [SerializeField] private float focusDistance = 0.3f; 
    [SerializeField] private float normalDistance = 10f; 

    private MeshRenderer _tableNoteRenderer;
    private Collider _tableNoteCollider;
    public bool IsNoteTaken { get; private set; } = false; 
    public bool HasReadNote { get; private set; } = false; 

    private float _nextActionTime = 0f;

    private void Awake()
    {
        if (stickyNoteOnTable != null)
        {
            if (audioSource == null) 
                audioSource = GetComponent<AudioSource>();

            _tableNoteRenderer = stickyNoteOnTable.GetComponent<MeshRenderer>();
            _tableNoteCollider = stickyNoteOnTable.GetComponent<Collider>();
        }

        if (globalVolume.profile.TryGet<DepthOfField>(out var dof))
        {
            _dof = dof;
        }
    }

    private void Update()
    {
        if (IsNoteTaken && Input.GetKeyDown(KeyCode.F))
        {
            if (Time.time >= _nextActionTime) PutNoteBack();
        }
        
        float targetDistance = IsNoteTaken ? focusDistance : normalDistance;
        if (_dof != null)
        {
            _dof.focusDistance.value = Mathf.Lerp(_dof.focusDistance.value, targetDistance, Time.deltaTime * transitionSpeed);
        }
    }

    public void TakeNote()
    {
        if (IsNoteTaken || Time.time < _nextActionTime) return;

        IsNoteTaken = true;
        HasReadNote = true;
        
        if (doorToUnlock != null)
        {
            doorToUnlock.UnlockDoor();
        }

        playerController.canMove = false;
        audioSource.PlayOneShot(soundEffect);   
        _nextActionTime = Time.time + actionCooldown;

        if (_tableNoteRenderer != null) _tableNoteRenderer.enabled = false;
        if (_tableNoteCollider != null) _tableNoteCollider.enabled = false;

        if (textOnBlackboard != null)
        {
            textOnBlackboard.SetActive(false);
        }

        if (stickyNoteOnCamera != null) stickyNoteOnCamera.SetActive(true);
    }

    private void PutNoteBack()
    {
        IsNoteTaken = false; 
        playerController.canMove = true;
        audioSource.PlayOneShot(soundEffect);
        _nextActionTime = Time.time + actionCooldown;

        if (_tableNoteRenderer != null) _tableNoteRenderer.enabled = true;
        if (_tableNoteCollider != null) _tableNoteCollider.enabled = true;

        if (stickyNoteOnCamera != null) stickyNoteOnCamera.SetActive(false);
    }
}