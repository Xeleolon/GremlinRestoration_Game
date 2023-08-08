using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    #region Interaction
[System.Serializable]
public class Interact
{
    [System.Serializable]
    public class WandData
    {
        public string name;
        public GameObject wand;
        public string animation;
    }
    [Tooltip("1 for repair, 2 for destory, 3 for replinish")]
    [Range(1, 3)]
    public int state = 1;
    float tempState;
    //[Tooltip("Bool For game to know to input methods for controller or keyboard")]
    //[SerializeField] private bool controlerActive = false;
    [Tooltip("how long before player can interact agian")]
    [SerializeField] private float refresh = 3;
    public bool fireRay = true;
    [Tooltip("How fast the scrool wheel takes to change to the next state")]
    public float scrollWheelSpeed = 10;
    private float clock;
    private Transform player;
    public GameObject centerSprite;
    public WandData[] wands = new WandData[4];
    private bool interactiveActive = true;
    private InputAction fire;
    private InputAction interactionOne;
    private InputAction interactionTwo;
    private InputAction interactionThree;
    private InputAction scrollwheel;
    public void EnableInteraction(PlayerInputActions playerControls)
    {
        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += ShootRay;

        interactionOne = playerControls.Player.InteractionOne;
        interactionOne.Enable();
        interactionOne.performed += InteractOne;

        interactionTwo = playerControls.Player.InteractionTwo;
        interactionTwo.Enable();
        interactionTwo.performed += InteractTwo;

        interactionThree = playerControls.Player.InteractionThree;
        interactionThree.Enable();
        interactionThree.performed += InteractThree;

        scrollwheel = playerControls.UI.ScrollWheel;
        scrollwheel.Enable();
    }
    public void DisableInteraction()
    {
        fire.Disable();
        interactionOne.Disable();
        interactionTwo.Disable();
        interactionThree.Disable();
        scrollwheel.Disable();

    }
    public void WandsNotNull()
    {
        for(int i = 0; i < wands.Length; i ++)
        {
            if (wands[i] == null)
            {
                wands[i] = new WandData();
            }
        }
    }
    public void UpdateInteractions(bool active)
    {
        interactiveActive = active;
        if (interactiveActive && !fireRay)
        {
            if (clock <= 0)
            {
                fireRay = true;
                if (centerSprite != null && !centerSprite.activeSelf)
                {
                    centerSprite.SetActive(true);
                }
            }
            else
            {
                clock -= 1 * Time.deltaTime;
            }
        }
    }
    private void ShootRay(InputAction.CallbackContext context)
    {
        if (interactiveActive && fireRay)
        {
                //Debug.Log("Fire");
            Ray ray;
                
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 100, 10))
            {
                //Debug.Log("hit " + hit.collider.gameObject.name);
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                //Debug.Log(interactable);


                if (interactable != null)
                {
                    if (player == null)
                    {
                        player = GameObject.FindWithTag("Player").transform;
                    }
                    //Debug.Log("Comformed hit");
                    if (interactable.OnInteract(player, state))
                    {
                        if (interactable.acheiveGoal != null)
                        {
                            PlayAnimation(interactable.acheiveGoal.type);
                        }
                        else
                        {
                            Debug.Log("Bug beating to interacting with the interacable before it has chance to make intisise item look into");
                        }
                        fireRay = false;
                        if (centerSprite != null && centerSprite.activeSelf)
                        {
                            centerSprite.SetActive(false);
                        }
                        clock = refresh;
                    }
                }
            }
        }
    }
    void PlayAnimation(int type)
    {
        if (type == 0 || type == state)
        {

            if ( wands[type].wand != null )
            {
                Animator animator = wands[type].wand.GetComponent<Animator>();
                if (animator != null && wands[type].animation != "")
                {
                    Debug.Log("animation state = " + type);
                    animator.Play(wands[type].animation);
                }
            }
        }
    }
    public void InteractOne(InputAction.CallbackContext context)
    {
        state = 1;
        NewState();
    }

    public void InteractTwo(InputAction.CallbackContext context)
    {
        state = 2;
        NewState();
    }

    public void InteractThree(InputAction.CallbackContext context)
    {
        state = 3;
        NewState();
    }
    public void ScrollWheel()
    {
        Vector2 scrollwheelInput = scrollwheel.ReadValue<Vector2>();
        //Debug.Log("scrollwheel Input = " + scrollwheelInput);
        if (interactiveActive && scrollwheelInput.y != 0)
        {
            tempState += (scrollwheelInput.y * scrollWheelSpeed);
            //Debug.Log("Dectecting ScrollWheel, which = " + tempState);
            if (tempState >= 1)
            {
                state += 1;
                if (state <= 0)
                {
                    state = 3;
                }
                else if (state >= 4)
                {
                    state = 1;
                }
                tempState = 0;
                NewState();
            }
            else if (tempState <= -1)
            {
                state -= 1;
                if (state <= 0)
                {
                    state = 3;
                }
                else if (state >= 4)
                {
                    state = 1;
                }
                tempState = 0;
                NewState();
            }
        }

    }

    void NewState()
    {
        switch (state)
        {
            case 1:
            LevelManager.instance.ChangeInteractUI(state);
            
            if (wands[1].wand != null && !wands[1].wand.activeSelf)
            {
                wands[1].wand.SetActive(true);
            }

            if (wands[2].wand != null && wands[2].wand.activeSelf)
            {
                wands[2].wand.SetActive(false);
            }

            if (wands[3].wand != null && wands[3].wand.activeSelf)
            {
                wands[3].wand.SetActive(false);
            }
            break;

            case 2:
            LevelManager.instance.ChangeInteractUI(state);
            if (wands[1].wand != null && wands[1].wand.activeSelf)
            {
                wands[1].wand.SetActive(false);
            }

            if (wands[2].wand != null && !wands[2].wand.activeSelf)
            {
                wands[2].wand.SetActive(true);
            }
            
            if (wands[3].wand != null && wands[3].wand.activeSelf)
            {
                wands[3].wand.SetActive(false);
            }
            break;

            case 3:
            LevelManager.instance.ChangeInteractUI(state);
            if (wands[1].wand != null && wands[1].wand.activeSelf)
            {
                wands[1].wand.SetActive(false);
            }

            if (wands[2].wand != null && wands[2].wand.activeSelf)
            {
                wands[2].wand.SetActive(false);
            }
            
            if (wands[3].wand != null && !wands[3].wand.activeSelf)
            {
                wands[3].wand.SetActive(true);
            }
            break;
            default:
            
            Debug.LogWarning("interaction state = " + state);
            break;
            
        }
    }
}
#endregion
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
    [Range(1, 20)]
    [SerializeField] private float cameraSensitivity;
    [Tooltip("The Maximum angle from the horizon the player can rotote, in degrees")]
    [SerializeField] private float cameraMaxVerticalAngleFromHorizon;
    [Tooltip("The period to wait until resetting the input value. Set this as low as possible without encountering stuttering from camera")]
    [SerializeField] private float cameraInputLagPeriod;
    private GameObject mainCamera;
    private Vector2 cameraRotation;
    private Vector2 cameraVelocity;
    private Vector2 cameraLastInputEvent;
    private float cameraInputLagTimer;
    private InputAction look;
    public void EnableCamera(Transform player, PlayerInputActions playerControls)
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
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
}
#endregion

    [SerializeField] public Interact interactions;
    [Header("Respawn/Death")]
    [Tooltip("How long Before Player Respawns")]
    [SerializeField] private float respawnLength = 5;
    private float respawnClock = 0;
    [Tooltip("if true input will work otherwise player isn't active")]
    public bool interactActive = true;
    private bool playerDead = false;
    private Vector3 lastCheckPoint;
    [SerializeField] private GameObject corpse;
    LevelManager levelManager;

    [Header("Player Movement")]
    
    [SerializeField] private float speed = 3;


    [SerializeField] private float jumpForce = 3;
    [SerializeField] private float jumpCooldown = 1;
    [SerializeField] private float airMultiplier = 1;
    private bool readyToJump;


    [SerializeField] private float groundDrag;

    [SerializeField] private float maxSlopeAngle = 45;

    private Vector3 velocity;
    private Vector3 moveDirection;
    private bool exitingSlope;

    private Rigidbody rb;

    private RaycastHit slopeHit;

    [Header ("Ground Check")]
    [SerializeField] private float playerHeight = 2;
    [SerializeField] private LayerMask whatIsGround;
    bool grounded;

    #region Enable & Disable
    //Input System
    private PlayerInputActions playerControls; //this is the script which holds all the inpuct actions
    private InputAction move;
    private InputAction sprint;
    private InputAction jumpInput;

    public CameraControls cameraControls;
    void OnValidate() //only calls if change when script reloads or change in value
    {
        interactions.WandsNotNull();
    }
    void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    void OnEnable()
    {
        cameraControls.EnableCamera(transform, playerControls);
        interactions.EnableInteraction(playerControls);
        
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
        interactions.DisableInteraction();
        cameraControls.DisableCamera();
    }

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //cameraControls.EnableCamera(transform, playerControls);
        lastCheckPoint = transform.position;
        levelManager = LevelManager.instance;
        LevelManager.instance.ChangeInteractUI(interactions.state);
        interactions.WandsNotNull();
        ResetJump();
        //rotationFreaze = new GameObject("DontDestoryPlayerRotation");
        //rotationFreaze.AddComponent<Rigidbody>();

    }

    void Update()
    {
        interactions.UpdateInteractions(interactActive);
        interactions.ScrollWheel();
        if (interactActive)
        {
            rb.useGravity = !OnSlope();
            GroundCheck();
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
                PlayerChat.instance.NewMessage("Player Respawned");
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

            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection() * speed * 20f, ForceMode.Force);

                if(rb.velocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }
            else if (grounded)
            {
                rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
            }
            else if (!grounded)
            {
                rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
            }

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
            respawnClock = respawnLength;
            Debug.Log("Player Died");
            PlayerChat.instance.NewMessage("Player Died");
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
        velocity.x = 0;
        velocity.z = 0;
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
        PlayerChat.instance.NewMessage("Check Point Saved");
    }
    #endregion
    #region Movement
    void MovementInputs()
    {
        Vector2 moveInputs = move.ReadValue<Vector2>(); //collector data from input before comparing as have a need to split the data into x and y
        
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
        return Vector3.ProjectOnPlane(transform.TransformDirection(moveDirection), slopeHit.normal).normalized;
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

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
        if (jumpInput.ReadValue<float>() > 0 && readyToJump && grounded)
        {
            exitingSlope = true;
            readyToJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);

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
    

    #endregion
}