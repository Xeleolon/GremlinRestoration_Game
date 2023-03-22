using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [Header("Interactions")]
    [Tooltip("1 for repair, 2 for destory, 3 for replinish")]
    [Range(1, 3)]
    public int interactionState = 0;
    //[Tooltip("Bool For game to know to input methods for controller or keyboard")]
    //[SerializeField] private bool controlerActive = false;
    [Tooltip("how long before player can interact agian")]
    [SerializeField] private float interactionRefresh = 3;
    private bool canInteract = true;
    private float interactTimer;
    float refVelocity = 0.0f;



    [Header("Player Movement")]
    public float speed = 3;
    [Tooltip("the rate for velocity to reach speed")]
    [SerializeField]private float acceleration = 0.3f;
    public float sprintSpeed = 6;
    public float jump = 1;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    
    [SerializeField] private bool onGround = true;
    private bool holdingJump = false; //is the player still holding jump when they land
    //Movement
    private Vector3 velocity;
    private Rigidbody rb;


    #region CameraMomevementVarables
    [Header("Camera Movement")]
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
    #endregion


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = GameObject.FindWithTag("MainCamera");
    }
    #region OnEnable
    void OnEnable()
    {
        //camera Elements
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
    #endregion

    void Update()
    {
        MovementInputs();
        
        
        JumpFunction();

        if (Input.GetAxisRaw("InteractOne") != 0 && interactionState != 1)
        {
            interactionState = 1;
            PlayerChat.instance.NewMessage(new string("interactionState has change to " + interactionState));
        }

        if (Input.GetAxisRaw("InteractTwo") != 0 && interactionState != 2)
        {
            interactionState = 2;
            PlayerChat.instance.NewMessage(new string("interactionState has change to " + interactionState));
        }

        if (Input.GetAxisRaw("InteractThree") != 0 && interactionState != 3)
        {
            interactionState = 3;
            PlayerChat.instance.NewMessage(new string("interactionState has change to " + interactionState));
        }

        #region InteractwithObject
        if (canInteract)
        {
            if (Input.GetAxisRaw("Fire1") != 0 )
            {
                //Debug.Log("Fire");
                Ray ray;
                
                ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        if (interactable.OnInteract(transform, interactionState))
                        {
                            canInteract = false;
                            interactTimer = interactionRefresh;
                        }
                    }
                }
            }
        }
        else
        {
            if (interactTimer <= 0)
            {
                canInteract = true; 
            }
            else
            {
                interactTimer -= 1 * Time.deltaTime;
            }
        }
        #endregion

        #region CameraMovement
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
        #endregion
    }
    void FixedUpdate()
    {
        //if (velocity.x != 0 || velocity.y != 0 || velocity.y != 0)
        //{
        rb.MovePosition(rb.position + transform.TransformDirection(velocity) * Time.fixedDeltaTime);
        //}
    }
    #region CameraMovementFuctions
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
    #endregion
    #region Movement
    void MovementInputs()
    {
        if (Input.GetAxisRaw("Vertical") > 0) //Y for Vertival movement
        {
            if (Input.GetAxisRaw("Sprint") > 0)
            {
                velocity.z = Mathf.SmoothDamp(velocity.z, sprintSpeed, ref refVelocity, acceleration);
            }
            else
            {
                velocity.z = Mathf.SmoothDamp(velocity.z, speed, ref refVelocity, acceleration);
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
    }


    void JumpFunction()
    {
        if (Input.GetAxisRaw("Jump") > 0 && onGround && !holdingJump)
        {
           
            rb.velocity = Vector3.up * jump;
            onGround = false;
        }
        
        if (rb.velocity.y <= 0 && !onGround)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y >= 0 && !Input.GetButton("Jump") && !onGround)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        if (holdingJump && !Input.GetButton("Jump"))
        {
            holdingJump = false;
        }

    }
    //attached to a trigger underplayer to decect when the player leaves the ground
    void OnTriggerEnter()
    {
        if (!onGround)
        {
            if (Input.GetButton("Jump"))
            {
                holdingJump = true;
            }
            onGround = true;
            
            //rb.velocity = 0;
        }
    }

    void OnTriggerExit()
    {
        if (onGround)
        {
            onGround = false;
        }
    }
    #endregion
}
