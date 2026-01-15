using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("UI and References")]
    [SerializeField] private TextMeshPro levelNumberText;
    [SerializeField] private AnomalyController anomalyController;
    [SerializeField] private LightsController lightsController;
    [SerializeField] private PPController ppController;
    [SerializeField] private SoundsController soundsController; 
    [SerializeField] private Door door;

    [Header("Settings")]
    [SerializeField] private float darkDuration = 1.5f;

    private int _currentLevelNumber = 1;
    private bool _isChangingLevel = false;

    private void Start()
    {
        if(ppController != null) ppController.ResetPPAmount();
        
        _currentLevelNumber = 1;
        UpdateLevelText();

        anomalyController.ResetAllAnomalies();
        anomalyController.SetAnomalies(_currentLevelNumber); 
    }

    public void NextLevel()
    {
        if (!_isChangingLevel) StartCoroutine(ChangeLevelState(true));
    }

    public void RestartGame()
    {
        if (!_isChangingLevel) StartCoroutine(ChangeLevelState(false));
    }

    private IEnumerator ChangeLevelState(bool isNextLevel)
    {
        _isChangingLevel = true;
        
        if(ppController != null) ppController.SetUpdateStatus(false);
        if(lightsController != null) lightsController.TurnOffLights();

        if (door != null)
        {
            door.Close();
            door.SetLevelTransitionLock(true);
        }
        
        yield return new WaitForSeconds(darkDuration);

        if (isNextLevel)
        {
            if (_currentLevelNumber >= 8)
            {
                SceneManager.LoadScene("WinScene");
                yield break; 
            }
            else
            {
                _currentLevelNumber++;
            }
        }
        else
        {
            _currentLevelNumber = 1;
        }

        UpdateLevelText();

        anomalyController.ResetAllAnomalies();
        anomalyController.SetAnomalies(_currentLevelNumber); 
    
        if(lightsController != null) lightsController.StopFlicker(); 

        if (soundsController != null)
        {
            soundsController.ResetAllAudio();
        }

        if (door != null)
        {
            door.SetLevelTransitionLock(false); 
        }

        _isChangingLevel = false;
        if(ppController != null) ppController.SetUpdateStatus(true);
    }

    private void UpdateLevelText()
    {
        if (levelNumberText != null)
            levelNumberText.text = ("Day: " + _currentLevelNumber.ToString());
    }
}