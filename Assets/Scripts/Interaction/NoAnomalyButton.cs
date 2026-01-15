using System.Collections;
using UnityEngine;

public class NoAnomalyButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AnomalyController anomalyController;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private StickyNoteController stickyNoteController;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect;

    [Header("Settings")]
    [SerializeField] private float actionCoolDown;

    private void Awake()
    {
        anomalyController = FindAnyObjectByType<AnomalyController>();
        levelManager = FindAnyObjectByType<LevelManager>();
        stickyNoteController = FindAnyObjectByType<StickyNoteController>();
    }

    public void ButtonPressed()
    {
        if (stickyNoteController != null && !stickyNoteController.HasReadNote)
        {
            return;
        }

        StartCoroutine(StartAction());
    }

    private IEnumerator StartAction()
    {
        if (audioSource != null && soundEffect != null)
            audioSource.PlayOneShot(soundEffect);

        yield return new WaitForSeconds(actionCoolDown);

        if(!anomalyController.IsThereAnomaly())
        {
            levelManager.NextLevel();
        }
        else
        {
            levelManager.RestartGame();
        }
    }
}