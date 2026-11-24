using UnityEngine;

public class testMovement : MonoBehaviour
{

    public float speed;
    public float strafeSpeed;
    

    public Rigidbody hips;
    public bool isGrounded;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hips = GetComponent<Rigidbody>();
    }


    private void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {

            if (Input.GetKey(KeyCode.LeftShift))
            {
                hips.AddForce(hips.transform.forward * speed * 1.5f);
            }

            else
            {
                hips.AddForce(hips.transform.forward * speed);
            }

        }

        if (Input.GetKey(KeyCode.A))
        {
            hips.AddForce(-hips.transform.right * strafeSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            hips.AddForce(-hips.transform.forward * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            hips.AddForce(hips.transform.right * strafeSpeed);
        }


    }
}
