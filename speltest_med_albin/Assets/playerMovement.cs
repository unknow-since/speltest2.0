using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;
    public float gravity = -20f;
    public float jumpHeight = 1.5f;
    public float jumpTimes = 1;
    float Jumped = 0;
    bool isCrouching = false;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get keyboard input
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            input.x = (Keyboard.current.dKey.isPressed ? 1 : 0)
                     - (Keyboard.current.aKey.isPressed ? 1 : 0);

            input.y = (Keyboard.current.wKey.isPressed ? 1 : 0)
                     - (Keyboard.current.sKey.isPressed ? 1 : 0);
        }

        // Turn input into a direction
        Vector3 direction =
            transform.right * input.x +
            transform.forward * input.y;

        // Choose acceleration or deceleration
        float currentAcceleration = input.magnitude > 0
            ? acceleration
            : deceleration;

        // Gradually reach the desired speed
        Vector3 targetVelocity = direction * speed;

        velocity.x = Mathf.MoveTowards(
            velocity.x,
            targetVelocity.x,
            currentAcceleration * Time.deltaTime
        );

        velocity.z = Mathf.MoveTowards(
            velocity.z,
            targetVelocity.z,
            currentAcceleration * Time.deltaTime
        );

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        if (Keyboard.current != null &&
       Keyboard.current.spaceKey.wasPressedThisFrame && Jumped <= jumpTimes)
        {

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Jumped = Jumped + 1;
        }

        // Jump
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame &&
            controller.isGrounded)
        {
            Jumped = 0;
            Jumped = Jumped + 1;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        if (Keyboard.current != null &&
     Keyboard.current.shiftKey.wasPressedThisFrame && isCrouching == true)
        {
            isCrouching = false;
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.position = new Vector3(0f, 5f, 0f);
        }


        else if (Keyboard.current != null &&
           Keyboard.current.shiftKey.wasPressedThisFrame && isCrouching == false)
        {
            isCrouching = true;
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            transform.position = new Vector3(0f, 5f, 0f);
        }


        // Move the character
        controller.Move(velocity * Time.deltaTime);
    }
}
