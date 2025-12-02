using UnityEngine;

public class NoAnomalyButton : MonoBehaviour
{
    [SerializeField] private AnomalyController anomalyController;
    [SerializeField] private LevelManager levelManager;

    private void Awake()
    {
        anomalyController = FindAnyObjectByType<AnomalyController>();
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    public void ButtonPressed()
    {
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