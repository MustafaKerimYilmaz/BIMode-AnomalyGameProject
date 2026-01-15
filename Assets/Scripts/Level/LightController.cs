using UnityEngine;
using System.Collections;
using TMPro;

public class LightsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light[] lights;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject linkedTextObject; 

    [Header("Audio Settings")]
    [SerializeField] private AudioClip lightTurnOnSound;
    [SerializeField] private AudioClip lightTurnOffSound; 
    [SerializeField] private AudioClip flickeringLoopSound;

    [Header("Settings")]
    [SerializeField] private float _defaultIntensity = 5f;
    [SerializeField] private float _minIntensity = 0.5f;
    [SerializeField] private float _maxIntensity = 5f;
    [SerializeField] private float _flickerSpeed = 0.1f;
    [SerializeField] private Color enemyChasingColor;

    private Coroutine _flickerCoroutine;
    private Color _defaultColor;

    private bool _areLightsOn = true; 
    private bool _isEnemySequenceActive = false;

    private void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        if (lights != null && lights.Length > 0)
        {         
            _defaultIntensity = lights[0].intensity;
            _defaultColor = lights[0].color;
        }

        _areLightsOn = lights.Length > 0 && lights[0].intensity > 0;
    }

    public void TurnOnLights()
    {
        if (_areLightsOn) return;

        PlayOneShotSound(lightTurnOnSound);

        foreach (Light light in lights)
        {
            if (light != null) light.intensity = _defaultIntensity;
        }

        if (linkedTextObject != null) linkedTextObject.SetActive(true);

        _areLightsOn = true;
    }
    
    public void TurnOffLights()
    {
        if (!_areLightsOn) return;

        StopFlickerSound();
        PlayOneShotSound(lightTurnOffSound);

        foreach (Light light in lights)
        {
            if (light != null) light.intensity = 0;
        }

        if (linkedTextObject != null) linkedTextObject.SetActive(false);

        _areLightsOn = false;
    }

    public void StartFlicker()
    {
        if (_flickerCoroutine != null) StopCoroutine(_flickerCoroutine);
        
        PlayFlickerLoop();

        _flickerCoroutine = StartCoroutine(FlickerRoutine());
    }

    public void StopFlicker()
    {
        if (_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
            _flickerCoroutine = null;
        }

        StopFlickerSound();

        if (!_areLightsOn)
        {
            PlayOneShotSound(lightTurnOnSound);
        }

        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.intensity = _defaultIntensity;
                light.color = _defaultColor;
            }
        }

        if (linkedTextObject != null) linkedTextObject.SetActive(true);

        _areLightsOn = true;
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            float randomIntensity = Random.Range(_minIntensity, _maxIntensity);
            
            foreach (Light light in lights)
            {
                if (light != null) light.intensity = randomIntensity;
            }

            if (linkedTextObject != null)
            {
                linkedTextObject.SetActive(randomIntensity > (_maxIntensity * 0.2f));
            }

            yield return new WaitForSeconds(_flickerSpeed);
        }
    }

    public IEnumerator EnemyStartsFollow(float duration)
    {
        if (_isEnemySequenceActive) yield break;

        _isEnemySequenceActive = true;
        
        TurnOffLights(); 
        EnemyChasingColor();

        yield return new WaitForSeconds(duration);

        TurnOnLights(); 
        StartFlicker();

        _isEnemySequenceActive = false;
    }

    private void EnemyChasingColor()
    {
        foreach (Light light in lights)
        {
            light.color = enemyChasingColor;
        }
    }
    
    private void PlayOneShotSound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    private void PlayFlickerLoop()
    {
        if (flickeringLoopSound != null && audioSource != null)
        {
            if (audioSource.isPlaying && audioSource.clip == flickeringLoopSound) return;

            audioSource.clip = flickeringLoopSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopFlickerSound()
    {
        if (audioSource != null)
        {
            if (audioSource.clip == flickeringLoopSound)
            {
                audioSource.Stop();
                audioSource.clip = null;
                audioSource.loop = false;
            }
        }
    }
}