using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnomalyProp
{
    public string propName;
    public GameObject normalObject;
    public GameObject anomalyObject;
}

public class AnomalyController : MonoBehaviour
{
    [SerializeField] private GameObject chasingEnemy;
    public List<AnomalyProp> allProps;
    
    [Header("Settings")]
    [SerializeField] private float anomalySpawnChance = 50f;
    [SerializeField] private float enemySpawnChance = 20f;
    [Range(1, 10)]
    [SerializeField] private int historySize = 4;

    private bool _isThereAnomaly = false;
    private List<int> _lastAnomalyIndices = new List<int>(); 

    private void Start()
    {
        ResetAllAnomalies();
    }

    public void SetAnomalies(int currentDay)
    {   
        if (currentDay <= 1)
        {
            _isThereAnomaly = false;
            return;
        }

        float chance = Random.Range(0f, 100f);

        if(chance <= anomalySpawnChance)
        {
            float enemyChance = Random.Range(0f, 100f);

            if(enemyChance <= enemySpawnChance && !_lastAnomalyIndices.Contains(-1))
            {
                chasingEnemy.SetActive(true);
                _isThereAnomaly = true;
                
                AddToHistory(-1); 
            }
            else
            {
                int index = GetUniqueRandomIndex();

                if(index != -1 && allProps[index] != null)
                {
                    allProps[index].normalObject.SetActive(false);
                    allProps[index].anomalyObject.SetActive(true);

                    _isThereAnomaly = true;
                    AddToHistory(index);
                }
            }
        }
    }

    private void AddToHistory(int index)
    {
        _lastAnomalyIndices.Add(index);
        if (_lastAnomalyIndices.Count > historySize)
        {
            _lastAnomalyIndices.RemoveAt(0);
        }
    }

    private int GetUniqueRandomIndex()
    {
        if (allProps.Count == 0) return -1;

        List<int> availableIndices = new List<int>();

        for (int i = 0; i < allProps.Count; i++)
        {
            if (!_lastAnomalyIndices.Contains(i))
            {
                availableIndices.Add(i);
            }
        }

        if (availableIndices.Count == 0)
        {
            _lastAnomalyIndices.Clear();
            return Random.Range(0, allProps.Count);
        }

        int randomIndex = Random.Range(0, availableIndices.Count);
        return availableIndices[randomIndex];
    }

    public void ResetAllAnomalies()
    {
        if(chasingEnemy != null && chasingEnemy.activeSelf)
            chasingEnemy.SetActive(false);

        foreach (AnomalyProp prop in allProps)
        {
            if(prop.normalObject != null) prop.normalObject.SetActive(true);
            if(prop.anomalyObject != null) prop.anomalyObject.SetActive(false);
        }

        _isThereAnomaly = false;
    }

    public void SetAnomalyBool(bool state) => _isThereAnomaly = state;
    public bool IsThereAnomaly() => _isThereAnomaly;
}