using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float speed = 3;
    public float sprintSpeed = 6;
    public float jump = 1;
    public bool OnGround;
    //Movement
    private Vector3 velocity;
    private Rigidbody rb;
    //Camera Control
    [Tooltip("The rotation acceleration, in degrees / second")]
    [SerializeField] private Vector2 cameraAcceleration;
    [Tooltip("A mutipler to the input. Describes the maximum speed in degrees / second.")]
    [SerializeField] private Vector2 cameraSensitivity;
    [Tooltip("The Maximum angle from the horizon the player can rotote, in degrees")]
    [SerializeField] private float cameraMaxVerticalAngleFromHorizon;
    [Tooltip("The period to wait until resetting the input value. Set this as low as possible without encountering stuttering from camera")]
    [SerializeField] private float cameraInputLagPeriod;
    private GameObject mainCamera;
    private Vector2 cameraRotation;
    private Vector2 cameraVelocity;
    private Vector2 cameraLastInputEvent;
    private float cameraInputLagTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = GameObject.FindWithTag("MainCamera");
    }
    void OnEnable()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        cameraVelocity = Vector2.zero;
        cameraInputLagTimer = 0;
        cameraLastInputEvent = Vector2.zero;

        Vector3 euler = transform.localEulerAngles;
        euler.x = mainCamera.transform.localEulerAngles.x;
        if(euler.x >= 180)
        {
            euler.x -= 360;
        }
        euler.x = ClampCameraVerticalAngle(euler.x);

        transform.localEulerAngles = new Vector3(0, euler.y, euler.z);
        mainCamera.transform.localEulerAngles = new Vector3(euler.x,0 , 0);

        cameraRotation = new Vector2(euler.y, euler.x);
        
    }

    void Update()
    {
        if (OnGround)
        {
        MovementInputs();
        }



        // camera Velocity is currenty mouse Input scaled by desired sensitivity
        // this is the maximum velocity
        Vector2 cameraSpeed = GetMouseInput() * cameraSensitivity;

        // Calculate new rotation and store it for future changes
        cameraVelocity = new Vector2(
            Mathf.MoveTowards(cameraVelocity.x, cameraSpeed.x, cameraAcceleration.x * Time.deltaTime),
            Mathf.MoveTowards(cameraVelocity.y, cameraSpeed.y, cameraAcceleration.y * Time.deltaTime));
        
        cameraRotation += cameraVelocity * Time.deltaTime;

        cameraRotation.y = ClampCameraVerticalAngle(cameraRotation.y);

        // convert the camera rotation to euler angles 
        transform.localEulerAngles = new Vector3(0, cameraRotation.x, 0);
        mainCamera.transform.localEulerAngles = new Vector3(cameraRotation.y, 0 ,0);
    }
    void FixedUpdate()
    {
        //if (velocity.x != 0 || velocity.y != 0 || velocity.y != 0)
        //{
        rb.MovePosition(rb.position + transform.TransformDirection(velocity) * Time.fixedDeltaTime);
        //}
    }
    private float ClampCameraVerticalAngle(float angle)
    {
        return Mathf.Clamp(angle, -cameraMaxVerticalAngleFromHorizon, cameraMaxVerticalAngleFromHorizon);
    }
    private Vector2 GetMouseInput()
    {
        cameraInputLagTimer += Time.deltaTime;


        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));

        if ((Mathf.Approximately(0, mouseInput.x) && Mathf.Approximately(0, mouseInput.y)) == false || cameraInputLagTimer >= cameraInputLagPeriod)
        {
            cameraLastInputEvent = mouseInput;
            cameraInputLagTimer = 0;
        }

        
        return cameraLastInputEvent;
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
