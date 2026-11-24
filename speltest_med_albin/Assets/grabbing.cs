using UnityEngine;

public class grabbing : MonoBehaviour
{
    [Header("Controls")]
    [Tooltip("0 = Left Mouse Button, 1 = Right Mouse Button")]
    public int mouseButton = 0;

    [Header("Grab Settings")]
    public LayerMask grabbableLayers = ~0; // Default: everything
    public float breakForce = 3500f;       // High enough to hold torso weight, low enough to prevent physics glitches
    public float grabRadius = 0.15f;       // Detection sphere around the hand

    [Header("Optional Pull Assist")]
    public Rigidbody torsoRigidbody;       // Assign your square Torso here
    public float pullAssistForce = 15f;    // Gentle upward/forward lift when grabbing to help drag the torso

    private FixedJoint grabJoint;
    private bool isGrabbingInput;
    private Collider[] ownColliders;

    void Start()
    {
        // Cache all colliders on the character so the hand never grabs itself
        ownColliders = transform.root.GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        isGrabbingInput = Input.GetMouseButton(mouseButton);

        // Release the joint immediately when the player lets go of the button
        if (!isGrabbingInput && grabJoint != null)
        {
            ReleaseGrab();
        }
    }

    void FixedUpdate()
    {
        // 1. If holding button and not yet attached, check for nearby surfaces to grab
        if (isGrabbingInput && grabJoint == null)
        {
            TryGrabNearbySurface();
        }

        // 2. While holding onto something, apply a slight upward lift to help the arms support the torso
        if (grabJoint != null && torsoRigidbody != null)
        {
            torsoRigidbody.AddForce(Vector3.up * pullAssistForce, ForceMode.Force);
        }
    }

    private void TryGrabNearbySurface()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, grabRadius, grabbableLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider col in hits)
        {
            // Ignore the character's own body parts
            if (IsOwnCollider(col)) continue;

            // Attach a FixedJoint to lock the hand onto the surface or object
            grabJoint = gameObject.AddComponent<FixedJoint>();
            grabJoint.connectedBody = col.attachedRigidbody; // Null if grabbing static floor/walls (locks to world!)
            grabJoint.breakForce = breakForce;
            grabJoint.enableCollision = false;
            break;
        }
    }

    public void ReleaseGrab()
    {
        if (grabJoint != null)
        {
            Destroy(grabJoint);
            grabJoint = null;
        }
    }

    private bool IsOwnCollider(Collider col)
    {
        foreach (Collider own in ownColliders)
        {
            if (col == own) return true;
        }
        return false;
    }

    // Visualize the grab detection sphere in the Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
}
