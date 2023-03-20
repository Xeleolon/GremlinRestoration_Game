using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float speed = 3;
    public float sprintSpeed = 6;
    public float jump = 1;
    public bool OnGround;
    private Vector3 velocity;
    private Rigidbody rb;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (OnGround)
        {
        MovementInputs();
        }
    }
    void FixedUpdate()
    {
        //if (velocity.x != 0 || velocity.y != 0 || velocity.y != 0)
        //{
        rb.MovePosition(rb.position + transform.TransformDirection(velocity) * Time.fixedDeltaTime);
        //}
    }
    void MovementInputs()
    {
        if (Input.GetAxisRaw("Vertical") > 0) //Y for Vertival movement
        {
            if (Input.GetAxisRaw("Sprint") > 0)
            {
                velocity.z = sprintSpeed;
            }
            else
            {
                velocity.z = speed;
            }
        }
        else if (Input.GetAxisRaw("Vertical") < 0) //Y for Vertival movement
        {
            velocity.z = -speed;
        }
        else
        {
            velocity.z = 0;
        }

        if (Input.GetAxisRaw("Horizontal") > 0) //X for Horizontal movement
        {
            velocity.x = speed;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0) //X for Horizontal movement
        {
            velocity.x = -speed;
        }
        else
        {
            velocity.x = 0;
        }
        if (Input.GetAxisRaw("Jump") > 0)
        {
            velocity.y = jump;
        }
        else
        {
            velocity.y = 0;
        }
    }
}
