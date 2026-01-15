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
    [SerializeField] private float anomalySpawnChance = 50f;
    [SerializeField] private float enemySpawnChance = 20f;

    private bool _isThereAnomaly = false;

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

            if(enemyChance <= enemySpawnChance)
            {
                chasingEnemy.SetActive(true);
                _isThereAnomaly = true;
            }
            else
            {
                int index = Random.Range(0, allProps.Count);

                if(allProps[index] != null)
                {
                    allProps[index].normalObject.SetActive(false);
                    allProps[index].anomalyObject.SetActive(true);

                    _isThereAnomaly = true;
                }
            }
        }
    }

    public void ResetAllAnomalies()
    {
        if(chasingEnemy != null && chasingEnemy.activeSelf)
            chasingEnemy.SetActive(false);

        foreach (AnomalyProp prop in allProps)
        {
            if(prop.normalObject != null)
            {
                prop.normalObject.SetActive(true);
            }
            if(prop.anomalyObject != null)
            {
                prop.anomalyObject.SetActive(false);
            }
        }

        _isThereAnomaly = false;
    }

    public void SetAnomalyBool(bool state)
    {
        _isThereAnomaly = state;
    }

    public bool IsThereAnomaly()
    {
        return _isThereAnomaly;
    }
}