using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AnomalyController anomalyController;
    [SerializeField] private LightsController lightsController;
    [SerializeField] private SoundsController soundsController;
    [SerializeField] private Animator animator; 
    [SerializeField] private AudioSource audioSource;

    [Header("Speed Settings")]
    [SerializeField] private float maxSpeed = 13f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private float minDistance = 2f;  

    [Header("Sequence Settings")]
    [SerializeField] private AudioClip jumpscareSound;
    [SerializeField] private float freezeDuration = 5f;
    [SerializeField] private float pathUpdateInterval = 0.2f;

    [Header("Atiklik Settings")]
    [SerializeField] private float turnSpeed = 15f;

    [Header("Animation Thresholds")]
    [SerializeField] private float walkingSpeedThreshold = 0.1f;
    [SerializeField] private float runningSpeedThreshold = 3.5f;

    private Vector3 _defaultEnemyPosition;
    private quaternion _defaultEnemyRotation;

    private bool _isEnemyTriggered = false;
    private bool _canStartChasing = false;
    private WaitForSeconds _pathUpdateWait;
    private Coroutine _followRoutine;

    private void Awake()
    {
        if (anomalyController == null)
            anomalyController = FindAnyObjectByType<AnomalyController>();
        
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (playerController == null && player != null)
            playerController = player.GetComponent<PlayerController>();

        _defaultEnemyPosition = transform.position;
        _defaultEnemyRotation = transform.rotation;
        
        _pathUpdateWait = new WaitForSeconds(pathUpdateInterval);
    }

    private void OnEnable()
    {
        if(anomalyController != null)
        {
             anomalyController.SetAnomalyBool(true);
        }
        
        _isEnemyTriggered = false;
        _canStartChasing = false;

        if (enemy != null)
        {
            enemy.Warp(_defaultEnemyPosition);
            transform.rotation = _defaultEnemyRotation;
            enemy.ResetPath();
            enemy.velocity = Vector3.zero; 
        }
        
        UpdateAnimations();

        if (_followRoutine != null) StopCoroutine(_followRoutine);
        _followRoutine = StartCoroutine(FollowPathRoutine());
    }

    private void Update()
    {
        UpdateAnimations();

        if (!_isEnemyTriggered || !_canStartChasing) return;

        if (enemy.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(enemy.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }
    }

    private IEnumerator FollowPathRoutine()
    {
        while (true)
        {
            if (_isEnemyTriggered && _canStartChasing && enemy.isOnNavMesh)
            {
                enemy.SetDestination(player.position);
                SetEnemySpeed();
            }
            yield return _pathUpdateWait;
        }
    }

    private void SetEnemySpeed()
    {
        float _distance = Vector3.Distance(transform.position, player.position);
        float speedFactor = Mathf.InverseLerp(minDistance, maxDistance, _distance);
        float finalSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedFactor);

        enemy.speed = finalSpeed;
    }

    private void UpdateAnimations()
    {
        if (animator == null || enemy == null) return;

        float currentSpeed = enemy.velocity.magnitude;
        bool isRunning = currentSpeed > runningSpeedThreshold;
        bool isWalking = !isRunning && (currentSpeed > walkingSpeedThreshold);

        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isWalking", isWalking);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isEnemyTriggered) return; 

        if (other.CompareTag("Player"))
        {
            _isEnemyTriggered = true;
            StartCoroutine(TriggerSequenceRoutine());
        }
    }

    private IEnumerator TriggerSequenceRoutine()
    {
        if (playerController != null)
            playerController.ToggleControls(false);

        if (lightsController != null)
             StartCoroutine(lightsController.EnemyStartsFollow(freezeDuration));

        if (audioSource != null && jumpscareSound != null)
            audioSource.PlayOneShot(jumpscareSound);

        yield return new WaitForSeconds(freezeDuration);

        if (soundsController != null)
        {
            soundsController.StartChase();
        }

        if (playerController != null)
            playerController.ToggleControls(true);

        _canStartChasing = true;
    }
}