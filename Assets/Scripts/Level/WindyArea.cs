using UnityEngine;

public class WindyArea : MonoBehaviour
{
    [SerializeField] private SoundsController soundsController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soundsController.EnterWindyArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soundsController.ExitWindyArea();
        }
    }
}