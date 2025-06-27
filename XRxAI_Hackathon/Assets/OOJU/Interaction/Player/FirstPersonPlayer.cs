using UnityEngine;
using UnityEngine.InputSystem;
// Simple first-person player controller (WASD/arrow keys + Space to jump). Uses the new Input System.
[RequireComponent(typeof(CharacterController))]
public class FirstPersonPlayer : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    private float rotationY = 0f;
    private CharacterController controller;
    private Vector3 velocity;
    private Camera playerCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        // Add a camera if not present
        if (playerCamera == null)
        {
            GameObject camObj = new GameObject("PlayerCamera");
            camObj.transform.SetParent(transform);
            camObj.transform.localPosition = new Vector3(0, 0.6f, 0); // Adjusted height for camera pivot
            playerCamera = camObj.AddComponent<Camera>();
        }

        if (Application.isPlaying)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        // Do nothing if input devices are not present
        if (Keyboard.current == null || Mouse.current == null) return;

        bool isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Move
        Vector2 moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.y = 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y = -1;
        if (Keyboard.current.aKey.isPressed) moveInput.x = -1;
        if (Keyboard.current.dKey.isPressed) moveInput.x = 1;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move.normalized * speed * Time.deltaTime);

        // Jump
        if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Mouse look
        if (Application.isPlaying)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * 0.1f;
            transform.Rotate(0, mouseDelta.x, 0);
            rotationY -= mouseDelta.y;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);
            if (playerCamera != null)
                playerCamera.transform.localEulerAngles = new Vector3(rotationY, 0, 0);
        }
    }
}