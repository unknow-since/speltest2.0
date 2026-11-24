using UnityEngine;

public class ragdollCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Assign your character's Torso/Chest here
    public Vector3 targetOffset = new Vector3(0f, 0.5f, 0f);

    [Header("Orbit Settings")]
    public float distance = 4.0f;
    public float mouseSensitivity = 3.0f;
    public float minPitch = -60f;
    public float maxPitch = 75f;
    public float followSmoothness = 15f;

    private float yaw;
    private float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Read mouse input
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Calculate world-space camera rotation and position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focusPoint = target.position + targetOffset;
        Vector3 desiredPosition = focusPoint - (rotation * Vector3.forward * distance);

        // Smoothly follow the torso's position without inheriting its roll/tumbling
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSmoothness * Time.deltaTime);
        transform.rotation = rotation;
    }
}
