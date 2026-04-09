using UnityEngine;
using UnityEngine.InputSystem; // Bắt buộc phải có namespace này

public class XRDebugController : MonoBehaviour
{
    [Header("Cấu hình di chuyển")]
    public float moveSpeed = 3.0f;
    public float lookSensitivity = 0.1f;

    [Header("Tham chiếu")]
    public Transform cameraTransform;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void HandleRotation()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        // Đọc dữ liệu từ chuột theo Input System mới
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * lookSensitivity;

        rotationY += mouseDelta.x;
        rotationX -= mouseDelta.y;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    void HandleMovement()
    {
        // Đọc dữ liệu từ bàn phím (WASD)
        Vector2 moveInput = Vector2.zero;
        var kb = Keyboard.current;

        if (kb.wKey.isPressed) moveInput.y = 1;
        if (kb.sKey.isPressed) moveInput.y = -1;
        if (kb.aKey.isPressed) moveInput.x = -1;
        if (kb.dKey.isPressed) moveInput.x = 1;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        transform.position += move * moveSpeed * Time.deltaTime;
    }
}