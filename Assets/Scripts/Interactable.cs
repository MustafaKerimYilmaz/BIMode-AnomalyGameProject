using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private Outline _outline;

    public UnityEvent onInteraction;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        DisableOutline();
    }

    public void Interact()
    {
        onInteraction.Invoke();
    }

    public void DisableOutline()
    {
        _outline.enabled = false;
    }

    public void EnableOutline()
    {
        _outline.enabled = true;
    }
}