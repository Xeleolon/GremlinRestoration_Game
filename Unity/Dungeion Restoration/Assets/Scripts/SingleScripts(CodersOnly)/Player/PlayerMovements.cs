using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    #region Camera
[System.Serializable]
public class CameraControls
{
    [Header("Camera Movement")]
    //Camera Control
    [Tooltip("The rotation acceleration, in degrees / second")]
    [Range(1000, 10000)]
    [SerializeField] private float cameraAcceleration;
    [Tooltip("A mutipler to the input. Describes the maximum speed in degrees / second.")]
    [Range(1, 30)]
    [SerializeField] private float cameraSensitivity;
    [Tooltip("The Maximum angle from the horizon the player can rotote, in degrees")]
    [SerializeField] private float cameraMaxVerticalAngleFromHorizon;
    [Tooltip("The period to wait until resetting the input value. Set this as low as possible without encountering stuttering from camera")]
    [SerializeField] private float cameraInputLagPeriod;
    [SerializeField] private GameObject mainCamera;
    private Vector2 cameraRotation;
    private Vector2 cameraVelocity;
    private Vector2 cameraLastInputEvent;
    private float cameraInputLagTimer;
    private InputAction look;
    public void EnableCamera(Transform player, PlayerInputActions playerControls)
    {
        //mainCamera = GameObject.FindWithTag("MainCamera");
        cameraVelocity = Vector2.zero;
        cameraInputLagTimer = 0;
        cameraLastInputEvent = Vector2.zero;

        Vector3 euler = player.localEulerAngles;
        euler.x = mainCamera.transform.localEulerAngles.x;
        if(euler.x >= 180)
        {
            euler.x -= 360;
        }
        euler.x = ClampCameraVerticalAngle(euler.x);

        player.localEulerAngles = new Vector3(0, euler.y, euler.z);
        mainCamera.transform.localEulerAngles = new Vector3(euler.x,0 , 0);

        cameraRotation = new Vector2(euler.y, euler.x);

        look = playerControls.Player.Look;
        look.Enable();
    }

    public void DisableCamera()
    {
        look.Disable();
    }
    public void MoveCamera(Transform player)
    {
        //Debug.Log("Moving Camera");
        Vector2 cameraSpeed = GetMouseInput() * new Vector2(cameraSensitivity, cameraSensitivity);

        // Calculate new rotation and store it for future changes
        cameraVelocity = new Vector2(
            Mathf.MoveTowards(cameraVelocity.x, cameraSpeed.x, cameraAcceleration * Time.deltaTime),
            Mathf.MoveTowards(cameraVelocity.y, cameraSpeed.y, cameraAcceleration * Time.deltaTime));
        
        cameraRotation += cameraVelocity * Time.deltaTime;

        cameraRotation.y = ClampCameraVerticalAngle(cameraRotation.y);

        // convert the camera rotation to euler angles 
        player.localEulerAngles = new Vector3(0, cameraRotation.x, 0);
        mainCamera.transform.localEulerAngles = new Vector3(cameraRotation.y, 0 ,0);
    }
    public void FreazeCamera(Transform player) //Freaze rotation on pause
    {
        //if you want the camera to rotate on pause apply the rotation here.
        player.localEulerAngles = new Vector3(0, cameraRotation.x, 0);
        mainCamera.transform.localEulerAngles = new Vector3(cameraRotation.y, 0 ,0);
    }
    private float ClampCameraVerticalAngle(float angle)
    {
        //Debug.Log("Clamping camera");
        return Mathf.Clamp(angle, -cameraMaxVerticalAngleFromHorizon, cameraMaxVerticalAngleFromHorizon);
    }
    private Vector2 GetMouseInput()
    {
        cameraInputLagTimer += Time.deltaTime;


        Vector2 mouseInput = look.ReadValue<Vector2>();
        mouseInput.y = -mouseInput.y; //invert y

        if ((Mathf.Approximately(0, mouseInput.x) && Mathf.Approximately(0, mouseInput.y)) == false || cameraInputLagTimer >= cameraInputLagPeriod)
        {
            cameraLastInputEvent = mouseInput;
            cameraInputLagTimer = 0;
        }

        
        return cameraLastInputEvent;
    }
    public void SetSensitivity(float newSensitivity)
    {
        cameraSensitivity = newSensitivity;
    }
}
#endregion

    [Header("Respawn/Death")]
    [Tooltip("How long Before Player Respawns")]
    [SerializeField] private float respawnLength = 5;
    private float respawnClock = 0;
    [Tooltip("if true input will work otherwise player isn't active")]
    public bool interactActive = true;
    private InteractControl interactiveScript;
    private bool playerDead = false;
    private Vector3 lastCheckPoint;
    [SerializeField] private GameObject corpse;
    LevelManager levelManager;

    [Header("Player Movement")]
    
    [SerializeField] private float speed = 3;


    [SerializeField] private float jumpForce = 3;
    [SerializeField] private float jumpCooldown = 1;
    [SerializeField] private float airMultiplier = 1;
    private bool climbing;
    [SerializeField] private float climbSpeed = 3;
    private bool readyToJump;


    [SerializeField] private float groundDrag;

    [SerializeField] private float maxSlopeAngle = 45;
    [Tooltip("How long before slope caluciation apply")]

    private Vector3 moveDirection;
    private Vector2 moveInputs; //house the info collected from inputs
    private bool exitingSlope;

    private Rigidbody rb;

    private RaycastHit slopeHit;
    private float lastPlayerHeight;
    [Header ("Sliding")]
    [SerializeField] private float slideForce;
    private bool sliding = false;
    private int slideTicket = 0;

    [Header ("Ground Check")]
    [SerializeField] private float playerHeight = 2;
    [SerializeField] private LayerMask whatIsGround;
    [Header ("Animations")]
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private string hitGroundAnimation; 
    bool grounded;

    #region Enable & Disable
    //Input System
    private PlayerInputActions playerControls; //this is the script which holds all the inpuct actions
    private InputAction move;
    private InputAction sprint;
    private InputAction jumpInput;

    public CameraControls cameraControls;
    void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    void OnEnable()
    {
        cameraControls.EnableCamera(transform, playerControls);
        
        // Input enable have to do this for the new input system
        move = playerControls.Player.Move;
        move.Enable();

        sprint = playerControls.Player.Sprint;
        sprint.Enable();

        jumpInput = playerControls.Player.Jump;
        jumpInput.Enable();
    }

    void OnDisable()
    {
        //Input disable have to do this for the new input system
        move.Disable();
        sprint.Disable();
        jumpInput.Disable();
        cameraControls.DisableCamera();
    }

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameManager gameManager = GameManager.instance;
        //cameraControls.EnableCamera(transform, playerControls);
        lastCheckPoint = transform.position;
        levelManager = LevelManager.instance;
        interactiveScript = GetComponent<InteractControl>();
        cameraControls.SetSensitivity(gameManager.cameraSensitivity);
        lastPlayerHeight = transform.position.y;
        ResetJump();
        //rotationFreaze = new GameObject("DontDestoryPlayerRotation");
        //rotationFreaze.AddComponent<Rigidbody>();

    }

    void Update()
    {
        if (interactActive)
        {
            rb.useGravity = !OnSlope();
            GroundCheck();
            moveInputs = move.ReadValue<Vector2>(); //collector data from input before comparing as have a need to split the data into x and y
            MovementInputs();

            JumpFunction();

            cameraControls.MoveCamera(transform);
        }
        else if (playerDead)
        {
            if (respawnClock <= 0)
            {
                levelManager.DeathMenu(false);
                playerDead = false;
                interactActive = true;
                interactiveScript.interactActive = true;
                Dialogue dialogue = new Dialogue(gameObject.name, "Player Repawned", 0);
                DebugController.instance.AddLog(dialogue);
                
            }
            else
            {
                respawnClock -= 1 * Time.deltaTime;
            }
        }
        else
        {
            cameraControls.FreazeCamera(transform);
        }
    }
    void FixedUpdate()
    {
        if (interactActive)
        {

            if (sliding)
            {
                Debug.Log("SLiding");
                cameraAnimator.SetBool("Sliding", true);
                SlidingMovement();
            }
            else
            {
                cameraAnimator.SetBool("Sliding", false);
            }

            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection() * speed * 15f, ForceMode.Force);

                if(rb.velocity.y > 0)
                {
                    if (transform.position.y < lastPlayerHeight) //slope movement going directly down
                    {
                        rb.AddForce(Vector3.down * 60f, ForceMode.Force);
                    }
                    else //slope movement up adding down force so player not just running and jumping off the stairs, might add it to a sprint 
                    {
                        rb.AddForce(Vector3.down * 20f, ForceMode.Force);
                    }
                }
            }
            else if (climbing || grounded)
            {
                rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
            }
            else if (!grounded)
            {
                rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
            }

            lastPlayerHeight = transform.position.y;
        }
    }
    # region PlayerLife
    public void KillPlayer()
    {
        if (respawnClock <= 0 || !playerDead)
        {
            playerDead = true;
            levelManager.DeathMenu(true);
            GenerateCorpse(transform.position);
            
            MoveToCheckPoint(); //sepatated from kill player allowing me to move to the checkpoint without killing the player
            interactActive = false;
            interactiveScript.interactActive = false;
            respawnClock = respawnLength;
            Debug.Log("Player Died");
            Dialogue dialogue = new Dialogue(gameObject.name, "Player Died", 0);
            DebugController.instance.AddLog(dialogue);
        }
    }
    public bool CheckPlayerLife()
    {
        if (playerDead)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void MoveToCheckPoint()
    {
        transform.position = lastCheckPoint;
        rb.velocity = Vector3.zero;
    }
    private void GenerateCorpse(Vector3 CorpsePosition)
    {
        if (corpse != null)
        {
           GameObject tempCorpse = Instantiate(corpse, CorpsePosition, Quaternion.Euler(new Vector3(90, 0, 0)));
        }
    }
    public void NewCheckPoint(Vector3 CheckPoint)
    {
        CheckPoint.y = CheckPoint.y + (transform.localScale.y / 2);
        lastCheckPoint = CheckPoint;
        Debug.Log("Check Point Saved");
        Dialogue dialogue = new Dialogue(gameObject.name, "Check Point Saved", 0);
        DebugController.instance.AddLog(dialogue);
    }
    #endregion
    #region Movement
    void MovementInputs()
    {
        
        moveDirection = transform.forward * moveInputs.y + transform.right * moveInputs.x;

        //SpeedControl

        if (OnSlope() && !exitingSlope && rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
        else
        { 
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit velocity if needed
            if (flatVel.magnitude > speed)
            {
                Vector3 limitedVel = flatVel.normalized * speed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }

        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public void Sliding(bool state)
    {
        if (state)
        {
            sliding = true;
            slideTicket += 1;
        }
        else if (slideTicket <= 1)
        {
            sliding = false;
            slideTicket = 0;
        }
        else
        {
            slideTicket -= 1;
        }
    }

    private void SlidingMovement()
    {
        if (!OnSlope() || rb.velocity.y > -0.1f)
        {
            Vector4 inputDirection = transform.forward * moveInputs.y + transform.right * moveInputs.x;
    
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
        }
        else
        {
            rb.AddForce(GetSlopeMoveDirection() * slideForce, ForceMode.Force);
        }
    }

    private void GroundCheck()
    {
        bool oldGrounded = grounded;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (!oldGrounded && grounded && hitGroundAnimation != "")
        {
            cameraAnimator.Play(hitGroundAnimation);
        }
        if (grounded)
        {
                rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void JumpFunction()
    {
        if (jumpInput.ReadValue<float>() > 0)
        {
            if (climbing)
            {
                rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
            }
            else if (readyToJump && grounded)
            {
                exitingSlope = true;
                readyToJump = false;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    
                Invoke(nameof(ResetJump), jumpCooldown);
            }

        }

    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public void ForceOnGround(bool newOnGround)
    {
        grounded = newOnGround;
    }

    public void ClimbingOn(bool value)
    {
        climbing = value;
    }
    

    #endregion
}