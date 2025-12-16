using System.Collections;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] lights;

    [Header("UI and References")]
    [SerializeField] private TextMeshPro levelNumberText;
    [SerializeField] private AnomalyController anomalyController;

    [Header("Settings")]
    [SerializeField] private int defaultLevelNumber;
    [SerializeField] private float darkDuration = 1f;

    private int _currentLevelNumber;
    private bool _isChangingLevel = false;

    private void Start()
    {
        _currentLevelNumber = defaultLevelNumber;
        UpdateLevelText();
    }

    public void NextLevel()
    {
        if (_isChangingLevel) return;

        StartCoroutine(ChangeLevelState(true));
    }

    public void RestartGame()
    {
        if (_isChangingLevel) return;

        StartCoroutine(ChangeLevelState(false));
    }

    private IEnumerator ChangeLevelState(bool isNextLevel)
    {
        _isChangingLevel = true; //Lock the state for player cannot spam to the buttons

        SetLights(false); //turn off the lights
        yield return new WaitForSeconds(darkDuration);

        if (isNextLevel)
        {
            if(levelNumberText.text == "0")
            {
                Debug.Log("YOU WIN");
            }
            else
            {
                 _currentLevelNumber -= 1;
            }
        }
        else
        {
            _currentLevelNumber = defaultLevelNumber;
        }

        UpdateLevelText();

        //Chancing level state
        anomalyController.ResetAllAnomalies();
        anomalyController.SetAnomalies();

        SetLights(true);

        _isChangingLevel = false; // Unlock the state
    }

    private void SetLights(bool state)
    {
        foreach(GameObject light in lights)
        {
            light.SetActive(state);
        }
    }

    private void UpdateLevelText()
    {
        if(levelNumberText != null)
        {
            levelNumberText.text = _currentLevelNumber.ToString();
        }
    }
}
