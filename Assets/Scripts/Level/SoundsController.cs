using UnityEngine;
using System.Collections;

public class SoundsController : MonoBehaviour
{
    [Header("Environment Audio Sources")]
    [SerializeField] private AudioSource normalAmbienceSource; 

    [Tooltip("Windy Area Sounds")]
    [SerializeField] private AudioSource windAmbienceSource; 

    [Tooltip("Enemy Chase Sounds")]
    [SerializeField] private AudioSource chaseMusicSource; 
    
    [SerializeField] private float fadeDuration = 1.2f;

    [Header("Footsteps")]
    [SerializeField] private AudioSource walkSource;
    [SerializeField] private AudioSource runSource;
    [SerializeField] private float footstepFadeSpeed = 5f;

    private bool _isChasing = false;
    private bool _isWindy = false;

    private Coroutine activeFade;
    private float targetWalkVol, targetRunVol;

    private void Start()
    {
        normalAmbienceSource.volume = 1f;
        windAmbienceSource.volume = 0f;
        chaseMusicSource.volume = 0f;

        normalAmbienceSource.loop = true;
        windAmbienceSource.loop = true;
        chaseMusicSource.loop = true;

        normalAmbienceSource.Play();
        windAmbienceSource.Play();
        chaseMusicSource.Play();

        walkSource.volume = 0f;
        runSource.volume = 0f;
        walkSource.Play();
        runSource.Play();
    }

    private void Update()
    {
        walkSource.volume = Mathf.MoveTowards(walkSource.volume, targetWalkVol, Time.deltaTime * footstepFadeSpeed);
        runSource.volume = Mathf.MoveTowards(runSource.volume, targetRunVol, Time.deltaTime * footstepFadeSpeed);
    }

    public void UpdateFootsteps(bool isMoving, bool isRunning)
    {
        if (!isMoving) { targetWalkVol = 0f; targetRunVol = 0f; }
        else if (isRunning) { targetWalkVol = 0f; targetRunVol = 1f; }
        else { targetWalkVol = 1f; targetRunVol = 0f; }
    }

    public void StartChase()
    {
        if (_isChasing) return;
        _isChasing = true;
        UpdateAmbienceState();
    }

    public void StopChase()
    {
        if (!_isChasing) return;
        _isChasing = false;
        UpdateAmbienceState();
    }

    public void EnterWindyArea()
    {
        if (_isWindy) return;
        _isWindy = true;
        UpdateAmbienceState();
    }

    public void ExitWindyArea()
    {
        if (!_isWindy) return;
        _isWindy = false;
        UpdateAmbienceState();
    }

    public void ResetAllAudio()
    {
        _isChasing = false;
        _isWindy = false;
        
        UpdateAmbienceState();
    }

    private void UpdateAmbienceState()
    {
        if (activeFade != null) StopCoroutine(activeFade);
        activeFade = StartCoroutine(CrossfadeRoutine());
    }

    private IEnumerator CrossfadeRoutine()
    {
        float timer = 0;
        
        float startNormal = normalAmbienceSource.volume;
        float startWind = windAmbienceSource.volume;
        float startChase = chaseMusicSource.volume;
        
        float targetNormal = 0f;
        float targetWind = 0f;
        float targetChase = 0f;

        if (_isChasing)
        {
            targetChase = 1f;
        }
        else if (_isWindy)
        {
            targetWind = 1f;
        }
        else
        {
            targetNormal = 1f;
        }

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            normalAmbienceSource.volume = Mathf.Lerp(startNormal, targetNormal, t);
            windAmbienceSource.volume = Mathf.Lerp(startWind, targetWind, t);
            chaseMusicSource.volume = Mathf.Lerp(startChase, targetChase, t);
            
            yield return null;
        }

        normalAmbienceSource.volume = targetNormal;
        windAmbienceSource.volume = targetWind;
        chaseMusicSource.volume = targetChase;
    }
}