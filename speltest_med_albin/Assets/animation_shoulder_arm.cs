using UnityEngine;

public class animation_shoulder_arm : MonoBehaviour
{
    [Tooltip("Drag the matching bone from AnimatedTargetRig here")]
    public Transform targetBone;

    [Header("Controls (Leave at -1 if this joint stays active all the time, like an elbow)")]
    [Tooltip("0 = Left Mouse, 1 = Right Mouse, -1 = Always Active")]
    public int mouseButton = -1;

    [Header("Spring Settings")]
    public float activeSpring = 2500f;
    public float activeDamper = 150f;
    public float relaxedSpring = 30f;
    public float relaxedDamper = 5f;

    private ConfigurableJoint joint;
    private Quaternion initialLocalRotation;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        initialLocalRotation = transform.localRotation;
    }

    void FixedUpdate()
    {
        if (targetBone == null || joint == null) return;

        bool isActive = (mouseButton == -1) || Input.GetMouseButton(mouseButton);

        if (isActive)
        {
            SetDrive(activeSpring, activeDamper);
            // Copy the invisible animated bone's rotation into ConfigurableJoint space
            joint.targetRotation = Quaternion.Inverse(targetBone.localRotation) * initialLocalRotation;
        }
        else
        {
            SetDrive(relaxedSpring, relaxedDamper);
            joint.targetRotation = Quaternion.identity;
        }
    }

    private void SetDrive(float spring, float damper)
    {
        JointDrive drive = joint.slerpDrive;
        drive.positionSpring = spring;
        drive.positionDamper = damper;
        drive.maximumForce = Mathf.Infinity;
        joint.slerpDrive = drive;
    }
}
