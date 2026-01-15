using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinSceneController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private TypeWritterEffect typewriter;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource winSoundSource; 
    [SerializeField] private float soundDelay = 5f;     

    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Timing Settings")]
    [SerializeField] private float fadeStartTime = 4f; 
    [SerializeField] private float fadeSpeed = 1f;     

    private float _timer = 0f;
    private bool _soundPlayed = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        _timer = 0f;
        _soundPlayed = false;
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= soundDelay && !_soundPlayed)
        {
            if (winSoundSource != null)
            {
                winSoundSource.Play();
            }
            _soundPlayed = true;
        }

        if (_timer >= fadeStartTime && fadeImage != null)
        {
            Color c = fadeImage.color;

            c.a = Mathf.MoveTowards(c.a, 1f, Time.deltaTime * fadeSpeed);
            fadeImage.color = c;
        }

        if (typewriter != null && typewriter.IsFinished && Input.anyKeyDown)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}