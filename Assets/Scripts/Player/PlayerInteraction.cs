using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float playerReach = 3f;
    [SerializeField] private KeyCode interactionKey;
    private Interactable _currentInteractable;

    void Update()
    {
        if (GameMenuManager.isGamePaused) return;

        CheckInteraction();

        if(Input.GetKeyDown(interactionKey) && _currentInteractable != null)
        {
            _currentInteractable.Interact();
        }
    }

    private void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if(Physics.Raycast(ray, out hit, playerReach))
        {
            if(hit.collider.tag == "Interactable")
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if(_currentInteractable && newInteractable != _currentInteractable)
                {
                    DisableCurrentInteractable();
                }

                if(newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    private void SetNewCurrentInteractable(Interactable newInteractable)
    {
        _currentInteractable = newInteractable;
        _currentInteractable.EnableOutline();
    }

    private void DisableCurrentInteractable()
    {
        if(_currentInteractable)
        {
            _currentInteractable.DisableOutline();
            _currentInteractable = null;
        }
    }
}
