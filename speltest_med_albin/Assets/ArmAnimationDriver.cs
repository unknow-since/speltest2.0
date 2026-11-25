using UnityEngine;

public class ArmAnimationDriver : MonoBehaviour
{
    public Animator targetAnimator;   // Drag AnimatedTargetRig's Animator here
    public Transform cameraTransform; // Drag Main Camera here

    void Update()
    {
        if (targetAnimator == null || cameraTransform == null) return;

        // Convert camera pitch (-60 to +75 degrees) into a -1 to +1 blend value
        float pitch = cameraTransform.eulerAngles.x;
        if (pitch > 180f) pitch -= 360f; // Convert 0..360 to -180..180

        // Invert pitch so looking up (+1) plays Reach_Up and looking down (-1) plays Reach_Down
        float normalizedPitch = Mathf.Clamp(-pitch / 65f, -1f, 1f);

        targetAnimator.SetFloat("Blend", normalizedPitch);
    }
}
