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

        //if colliders with anything within player reach
        if(Physics.Raycast(ray, out hit, playerReach))
        {
            if(hit.collider.tag == "Interactable") // if looking at an interactable object
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                //if there is a currentInterectable and it is not the newInteractable
                if(_currentInteractable && newInteractable != _currentInteractable)
                {
                    DisableCurrentInteractable();
                }

                if(newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else // if new interactable is not enabled
                {
                    DisableCurrentInteractable();
                }
            }
            else // if not an interactable
            {
                DisableCurrentInteractable();
            }
        }
        else // if nothing in reach
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
