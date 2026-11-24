using UnityEngine;

public class armMovement : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    [Header("Controls")]
    [Tooltip("0 = Left Mouse Button, 1 = Right Mouse Button")]
    public int mouseButton = 0;

    [Header("Joint Spring Settings")]
    public float activeSpring = 800f;
    public float activeDamper = 50f;
    public float relaxedSpring = 20f;
    public float relaxedDamper = 5f;

    [Header("Bone Alignment")]
    [Tooltip("Adjust if your arm points sideways or backwards when reaching (e.g., (90,0,0) if Y is down the arm)")]
    public Vector3 aimAxisOffset = Vector3.zero;

    private ConfigurableJoint joint;
    private Quaternion initialLocalRotation;
    private bool isReaching;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();

        // Store the initial rotation relative to the torso before physics starts
        if (joint.connectedBody != null)
        {
            initialLocalRotation = Quaternion.Inverse(joint.connectedBody.transform.rotation) * transform.rotation;
        }
        else
        {
            initialLocalRotation = transform.localRotation;
        }
    }

    void Update()
    {
        isReaching = Input.GetMouseButton(mouseButton);
    }

    void FixedUpdate()
    {
        if (joint == null || cameraTransform == null || joint.connectedBody == null) return;

        if (isReaching)
        {
            SetJointDrive(activeSpring, activeDamper);
            AimArmInWorldSpace();
        }
        else
        {
            SetJointDrive(relaxedSpring, relaxedDamper);
            joint.targetRotation = Quaternion.identity; // Relax back to default pose
        }
    }

    private void AimArmInWorldSpace()
    {
        // 1. Get desired world rotation pointing along the camera's forward direction
        Quaternion targetWorldRotation = Quaternion.LookRotation(cameraTransform.forward, Vector3.up);

        // 2. Apply any bone-axis offset so the length of the arm points at the target
        targetWorldRotation *= Quaternion.Euler(aimAxisOffset);

        // 3. Convert world rotation into the torso's local space (works even if upside down or lying flat)
        Quaternion localTarget = Quaternion.Inverse(joint.connectedBody.transform.rotation) * targetWorldRotation;

        // 4. Convert to ConfigurableJoint targetRotation space
        joint.targetRotation = Quaternion.Inverse(localTarget) * initialLocalRotation;
    }

    private void SetJointDrive(float spring, float damper)
    {
        JointDrive drive = joint.angularXDrive;
        drive.positionSpring = spring;
        drive.positionDamper = damper;
        joint.angularXDrive = drive;
        joint.angularYZDrive = drive;
    }
}
