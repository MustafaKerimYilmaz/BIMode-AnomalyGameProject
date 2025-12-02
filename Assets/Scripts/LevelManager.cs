using NUnit.Framework;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro levelNumberText;
    [SerializeField] private int defaultLevelNumber;

    private int _currentLevelNumber;

    private void Start()
    {
        _currentLevelNumber = defaultLevelNumber;
    }

    public void NextLevel()
    {
        if(levelNumberText.text == "0")
        {
            // Win state
        }
        else
        {
            // Change Level State
            _currentLevelNumber -= 1;
            levelNumberText.text = _currentLevelNumber.ToString();
        }
    }

    public void RestartGame()
    {
        // Change Level State
        _currentLevelNumber = defaultLevelNumber;
        levelNumberText.text = defaultLevelNumber.ToString();
    }
}