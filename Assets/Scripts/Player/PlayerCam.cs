using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController; 
    [SerializeField] private Transform orientation;

    [Header("Settings")]
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    private float xRotation;
    private float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (GameMenuManager.isGamePaused) 
        {
            return; 
        }

        if (playerController != null && !playerController.canMove) 
            return;

        float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}