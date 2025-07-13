using UnityEngine;
public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Player transform
    public float followSpeed = 5f;

    [Header("Camera Rotation Settings")]
    public float mouseSensitivity = 3f;
    public float verticalRotationLimit = 80f;

    [Header("Camera Distance Settings")]
    public float defaultDistance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;
    public float zoomSpeed = 2f;

    private float currentVerticalAngle = 0f;
    private float currentHorizontalAngle = 0f;
    private float currentDistance;

    void Start()
    {
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize camera position
        currentDistance = defaultDistance;

        // If no target is set, try to find player
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        // Camera Rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Horizontal rotation (around Y-axis)
        currentHorizontalAngle += mouseX;

        // Vertical rotation (around X-axis)
        currentVerticalAngle -= mouseY;

        // Clamp vertical rotation to prevent excessive upward and downward movement
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, -10f, verticalRotationLimit);

        // Zoom with mouse scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance = Mathf.Clamp(currentDistance - scroll * zoomSpeed, minDistance, maxDistance);
    }


    void LateUpdate()
    {
        if (target == null) return;

        // Calculate camera rotation
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);

        // Calculate camera position
        Vector3 negativeDistance = new Vector3(0.0f, 0.0f, -currentDistance);
        Vector3 position = rotation * negativeDistance + target.position;

        // Smooth follow and look at target
        transform.rotation = rotation;
        transform.position = Vector3.Lerp(transform.position, position, followSpeed * Time.deltaTime);
    }
    void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle cursor lock
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
