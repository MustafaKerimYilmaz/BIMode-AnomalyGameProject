using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 100f;

    [Header("References")]
    private Rigidbody _rb;
    [SerializeField] private Transform cameraTransform;

    [Header("Input Settings")]
    private float _xInput;
    private float _yInput;
    private float _mouseX;
    private float _mouseY;

    private float _xRotation;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (GameMenuManager.isGamePaused) return;

        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");

        _mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        _mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        HandleMouseLock();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleMouseLock()
    {
        transform.Rotate(Vector3.up * _mouseX);
        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void MovePlayer()
    {
        Vector3 moveDirection = transform.right * _xInput + transform.forward * _yInput;
        Vector3 targetVelocity = moveDirection.normalized * moveSpeed;
        targetVelocity.y = _rb.linearVelocity.y;
        _rb.linearVelocity = targetVelocity;
    }
}