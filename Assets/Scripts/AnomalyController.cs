using UnityEngine;

public class AnomalyController : MonoBehaviour
{
    private bool _isThereAnomaly = false;

    public void SetAnomalyBool(bool state)
    {
        _isThereAnomaly = state;
    }

    public bool IsThereAnomaly()
    {
        return _isThereAnomaly;
    }
}