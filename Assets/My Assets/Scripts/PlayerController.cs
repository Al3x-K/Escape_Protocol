using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 30f; 
    public float rotationSpeed = 720f; 
    public float mouseSensitivity = 2f; 
    [SerializeField] public Vector3 cameraPosition;

    private NavMeshAgent agent;
    private Camera mainCamera;
    private float yaw = 0f;
    private float pitch = 0f; 
    public float pitchLimit = 45f; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        agent.updateRotation = false; 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");  

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
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Adjust yaw and pitch based on mouse input.
        yaw += mouseX;
        pitch -= mouseY;

        // Clamp the pitch to prevent over-rotation.
        pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);

        // Rotate the character left and right based on yaw.
        Quaternion targetRotation = Quaternion.Euler(0, yaw, 0);
        transform.rotation = targetRotation;

        // Rotate the camera up and down based on pitch.
        mainCamera.transform.position = transform.position + cameraPosition;
        mainCamera.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

   
}
