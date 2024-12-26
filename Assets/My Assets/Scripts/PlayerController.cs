using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 30f; // Speed of the character.
    public float rotationSpeed = 720f; // Rotation speed in degrees per second.
    public float mouseSensitivity = 2f; // Sensitivity of mouse look.
    [SerializeField] public Vector3 cameraPosition;

    private NavMeshAgent agent;
    private Camera mainCamera;
    private float yaw = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        agent.updateRotation = false; // Prevent NavMeshAgent from controlling rotation.
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor for mouse look.
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleMovement()
    {
        // Get input for movement.
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right.
        float vertical = Input.GetAxis("Vertical");   // W/S or Up/Down.

        // Calculate movement direction relative to the camera.
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            // Move the character manually.
            agent.Move(moveDirection * movementSpeed * Time.deltaTime);

            // Rotate character smoothly towards movement direction if needed.
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleMouseLook()
    {
        // Get mouse movement.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        // Adjust yaw based on mouse input.
        yaw += mouseX;

        // Rotate the character based on yaw.
        Quaternion targetRotation = Quaternion.Euler(0, yaw, 0);
        transform.rotation = targetRotation;

        // Rotate the camera based on yaw.
        mainCamera.transform.position = transform.position + cameraPosition;
        mainCamera.transform.rotation = targetRotation;


    }
}
