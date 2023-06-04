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
            
            if (Physics.Raycast(ray, out hit, 100, 7))
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
    private float respawnTimer = 0;
    [Tooltip("if true input will work otherwise player isn't active")]
    public bool interactActive = true;
    private bool playerDead = false;
    private Vector3 lastCheckPoint;
    [SerializeField] private GameObject corpse;
    LevelManager levelManager;

    [Header("Player Movement")]
    public float speed = 3;
    [Tooltip("the rate for velocity to reach speed. Larger the Value is the faster you reach maxSpeed")]
    [SerializeField]private float acceleration = 0.3f;
    [Tooltip("the Added Speed to create the sprint")]
    [SerializeField]private float sprintSpeed = 6;
    [Tooltip("stopping spead")]
    [SerializeField]private float stopSpeed = 1;
    public float jump = 1;
    public string groundTag;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    
    [SerializeField] private bool onGround = true;
    private bool delayOnGround = false;
    private bool onRamp = false;
    private bool holdingJump = false; //is the player still holding jump when they land
    private bool collisionJump = false; //a bool that force jump to fall if the player collides and loses all motion in moveStateX or moveStateY
    //Movement
    private Vector3 velocity;
    float veritcalAcceleration = 0.0f; //container to track when at fall speed
    float horizontalAcceleration = 0.0f;
    bool forward = false; //movement states to check which way the player is current moving.
    bool backward = false;
    bool right = false;
    bool left = false;
    private Rigidbody rb;
    private int moveStateX = 6;
    private int moveStateY = 6;
    private GameObject rotationFreaze; //a empty gameObject which is holds the player rotation and freaze it postion
    private bool rotationFreazeMove = false;
    [Tooltip("for Collision")]


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
    #region Enable & Disable
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
        rotationFreaze = new GameObject("DontDestoryPlayerRotation");
        //rotationFreaze.AddComponent<Rigidbody>();

    }

    void Update()
    {
        interactions.UpdateInteractions(interactActive);
        interactions.ScrollWheel();
        if (interactActive)
        {
            if (!rotationFreazeMove)
            {
                if (collisionJump)
                {
                    collisionJump = false;
                }
                MovementInputs();
            }
            else
            {
                SlowDown();
            }
        Moving(moveStateX);
        Moving(moveStateY);
        
        JumpFunction();


        cameraControls.MoveCamera(transform);
        }
        else if (playerDead)
        {
            if (respawnTimer <= 0)
            {
                levelManager.DeathMenu(false);
                playerDead = false;
                interactActive = true;
                PlayerChat.instance.NewMessage("Player Respawned");
            }
            else
            {
                respawnTimer -= 1 * Time.deltaTime;
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
            if (!rotationFreazeMove)
            {
                rb.MovePosition(rb.position + transform.TransformDirection(velocity) * Time.fixedDeltaTime);
                if (moveStateX == 5 && moveStateY == 5 && onGround && onRamp)
                {
                    rb.velocity = Vector3.zero;
                }
            }
            else
            {
                //work if facing correct forward need to translate last position rotation to be new object rotation.
                rb.MovePosition(rb.position + rotationFreaze.transform.TransformDirection(velocity) * Time.fixedDeltaTime);
            }
        }
    }
    # region PlayerLife
    public void KillPlayer()
    {
        if (respawnTimer <= 0 || !playerDead)
        {
            playerDead = true;
            levelManager.DeathMenu(true);
            GenerateCorpse(transform.position);
            
            MoveToCheckPoint(); //sepatated from kill player allowing me to move to the checkpoint without killing the player
            interactActive = false;
            respawnTimer = respawnLength;
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
        if (moveInputs.y > 0) //Y for Vertival movement
        {
            if (!forward)
            {
                veritcalAcceleration = 0;
                forward = true;
            }
            else if (veritcalAcceleration < (1 + (sprintSpeed * 0.1)))
            {
                veritcalAcceleration += acceleration * Time.deltaTime;
            }

            if (sprint.ReadValue<float>() > 0)
            {
                moveStateX = 0;
            }
            else
            {
                if (veritcalAcceleration >= 1)
                {
                    veritcalAcceleration = 1;
                }
                
                moveStateX = 1;
            }
        }
        else if (moveInputs.y < 0) //Y for Vertival movement
        {
            if (!backward)
            {
                veritcalAcceleration = 1;
                backward = true;
            }
            else if(veritcalAcceleration > 0)
            {
                veritcalAcceleration -=acceleration * Time.deltaTime;
            }
           
           moveStateX = 2;
        }
        else
        {
            if (forward) 
            {
                veritcalAcceleration = 0;
                forward = false;
            }
            else if (backward)
            {
                veritcalAcceleration = 1;
                backward = false;
            }
            velocity.z = 0;
            moveStateX = 5;
            moveStateY = 5;
        }

        if (moveInputs.x > 0) //X for Horizontal movement
        {
            if (!right)
            {
                horizontalAcceleration = 0;
                right = true;
            }
            else if (horizontalAcceleration < 1)
            {
                horizontalAcceleration += acceleration * Time.deltaTime;
            }
            
            moveStateY = 3;
        }
        else if (moveInputs.x < 0) //X for Horizontal movement
        {
            if (!left)
            {
                horizontalAcceleration = 1;
                left = true;
            }
            else if (horizontalAcceleration > 0)
            {
                horizontalAcceleration -= acceleration * Time.deltaTime;
            }
            
            moveStateY = 4;
        }
        else
        {
            if (right)
            {
                right = false;
                horizontalAcceleration = 0;
            }
            else if (left)
            {
                left = false;
                horizontalAcceleration = 1;
            }
            velocity.x = 0;
            moveStateY = 5;
        }
    }
    void Moving(int state) //help to allow movement to be held while in free fall without reseting and changing
    {
        if (state < 5) // 5 for not to be used
        {
            switch (state)
        {
            case (0): // 0 for sprint forward
            velocity.z = Mathf.LerpUnclamped(0, speed, veritcalAcceleration);
            break;
            
            case (1): // 1 for forward
            velocity.z = Mathf.Lerp(0, speed, veritcalAcceleration);
            break; 

            case (2): // 2 for backwards
            velocity.z = Mathf.Lerp(-speed, 0, veritcalAcceleration);
            break;

            case (3): // 3 for right
            velocity.x = Mathf.Lerp(0, speed, horizontalAcceleration);
            break;

            case (4): // 4 for left
            velocity.x = Mathf.Lerp(-speed, 0, horizontalAcceleration);
            break;
        }
        }
    }
    void SlowDown()
    {
        if (forward && veritcalAcceleration != 0)
        {
            if (veritcalAcceleration <= 0.1)
            {
                veritcalAcceleration = 0;
                forward = false;
            }
            else
            {
                //Reduce VerticalAcceleration
                float newAcceleration = veritcalAcceleration - stopSpeed * Time.deltaTime;
                veritcalAcceleration = newAcceleration;
            }
        }
        else if (backward && veritcalAcceleration != 1)
        {
            if (veritcalAcceleration >= 0.9)
            {
                veritcalAcceleration = 1;
                backward = false;
            }
            else
            {
                //Reduce VerticalAcceleration
                float newAcceleration = veritcalAcceleration + stopSpeed * Time.deltaTime;
                veritcalAcceleration = newAcceleration;
            }
        }

        if (right && horizontalAcceleration != 0)
        {
            if (horizontalAcceleration <= 0.1)
            {
                horizontalAcceleration = 0;
                right = false;
            }
            else
            {
                //Reduce VerticalAcceleration
                float newAcceleration = horizontalAcceleration - stopSpeed * Time.deltaTime;
                horizontalAcceleration = newAcceleration;
            }
        }
        else if (left && horizontalAcceleration != 1)
        {
            if (horizontalAcceleration >= 0.9)
            {
                horizontalAcceleration = 1;
                left = false;
            }
            else
            {
                //Reduce VerticalAcceleration
                float newAcceleration = horizontalAcceleration + stopSpeed * Time.deltaTime;
                horizontalAcceleration = newAcceleration;
            }
        }
        
    }

    #endregion
    #region Jump
    void JumpFunction()
    {
        if (jumpInput.ReadValue<float>() > 0 && onGround && !holdingJump && !collisionJump) //Jump
        {
           
            rb.velocity = Vector3.up * jump;
            if (onGround)
            {
                rb.useGravity = true;
                //rb.isKinematic = false;
                onGround = false;
            }
        }
        
        if (rb.velocity.y <= 0 && onGround) //Failing from jump
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y >= 0 && jumpInput.ReadValue<float>() <= 0 && !onGround)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        if (holdingJump && jumpInput.ReadValue<float>() <= 0)
        {
            holdingJump = false;
        }

    }
    //attached to a trigger underplayer to decect when the player leaves the ground
    void OnTriggerEnter(Collider other)
    {
        if (!onGround)
        {
            if (other.tag == groundTag)
            {
                onGround = true;
                rb.useGravity = true;
                if (delayOnGround)
                {
                    delayOnGround = false;
                }
                if (jumpInput.ReadValue<float>() > 0)
                {
                    holdingJump = true;
                }
            }
            else if (other.tag == "Ramp")
            {
                if (delayOnGround)
                {
                    delayOnGround = false;
                }
                onRamp = true;
                onGround = true;
                rb.useGravity = false;
                if (jumpInput.ReadValue<float>() > 0)
                {
                    holdingJump = true;
                }
            }
            else if (other.tag == "MovingPlate")
            {
                delayOnGround = true;
                onGround = true;
                rb.useGravity = true;
                if (jumpInput.ReadValue<float>() > 0)
                {
                    holdingJump = true;
                }
            }
            //rb.isKinematic = true;

            
            //rb.velocity = 0;
        }
        
        rotationFreazeMove = false;
    }
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("On Ground Stay Decatating " + other);
        if (!onGround && other.tag != "Player")
        {
            if (other.tag == groundTag || other.tag == "MovingPlate")
            {
                onGround = true;
                rb.useGravity = true;
            }
            else if (other.tag == "Ramp")
            {
                onGround = true;
                onRamp = true;
                rb.useGravity = false;
            }
            else if (other.tag == "MovingPlate")
            {
                delayOnGround = true;
                onGround = true;
                rb.useGravity = true;
            }
        }

        rotationFreazeMove = false;
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("trying to leave ground");
        if (onGround)
        {
            if (other.tag == groundTag || other.tag == "Ramp" || other.tag == "MovingPlate")
            {
                onGround = false;
                rb.useGravity = true;
            }

            if (other.tag == "Ramp")
            {
                onRamp = false;
            }

        }
        rotationFreaze.transform.position = transform.position;
        rotationFreaze.transform.rotation = transform.rotation;
        rotationFreazeMove = true;
        //Debug.Log(gameObject.transform.rotation + " & rotationFreaze:" + rotationFreaze.transform.rotation);
    }
    public void ForceOnGround(bool state)
    {
        if (state)
        {
            onGround = true;
        }
        else
        {
            onGround = false;
            delayOnGround = true;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (rotationFreazeMove)
        {
            Debug.Log("Colsion Decected");
            Vector3 contactPoint = rotationFreaze.transform.TransformDirection(other.contacts[0].point);
            //Debug.Log(contactPoint + " and unchange: " + other.contacts[0].point);
            //Vector3 currentForward = rotationFreaze.transform.TransformDirection(transform.position);
            if (contactPoint.y <= transform.position.y + 1 && contactPoint.y >= transform.position.y - 1)
            {
                if (forward && collisionRayCast(new Vector2(0, 0.5f), contactPoint.y, true))
                {
                    Debug.Log("Killing Forward Momentem");
                    veritcalAcceleration = 0;
                    forward = false;
                    moveStateX = 5;
                }
                else if (backward && collisionRayCast(new Vector2(0, 0.5f), contactPoint.y, false))
                {
                    veritcalAcceleration = 1;
                    backward = false;
                    moveStateX = 5;
                }

                if (right && collisionRayCast(new Vector2(0.5f, 0 ), contactPoint.y, true))
                {
                    horizontalAcceleration = 0;
                    right = false;
                    moveStateY = 5;
                }
                else if (left && collisionRayCast(new Vector2(0.5f, 0), contactPoint.y, false))
                {
                    moveStateY = 5;
                    left = false;
                    horizontalAcceleration = 1;
                }
            }

            if (moveStateX >= 5 && moveStateY >= 5)
            {
                //stop jump if colision and all movement has being killed
                collisionJump = true;
            }
        }
    }
    

    bool collisionRayCast(Vector2 corners, float hieght, bool postive)
    {
        Debug.Log("Calculating new Ray");
        Ray corner1 = new Ray();
        Ray corner2 = new Ray();
        callacuteRay(true);
        callacuteRay(false);
        RaycastHit hit;

        if (Physics.Raycast(corner1, out hit, 10, 7))
        {
            //Debug.DrawRay(corner1, Color.yellow);
            Debug.Log("Hit with Corner 1");
            return true;
        }
        else if (Physics.Raycast(corner2, out hit, 10, 7))
        {
            Debug.Log("Hit with Corner 2");
            return true;
        }
        

        return false;
        //creates the calcutiation for booth ray
        void callacuteRay(bool firstRay)
        {
            rotationFreaze.transform.position = transform.position;
            Vector3 tempOrign = transform.position;
            tempOrign.y = hieght;
            if (firstRay)
            {
                tempOrign.x += corners.x;
                tempOrign.z += corners.y;
            }
            else
            {
                tempOrign.x += corners.x;
                tempOrign.z += corners.y;
            }

            Vector3 tempDirection = tempOrign;
            float makeNegative = 1;
            if (!postive)
            {
                makeNegative = -1;
            }
            if (corners.x == 0)
            {
                tempDirection.y += 0.3f * makeNegative;
            }
            else
            {
                tempDirection.x += 0.3f * makeNegative;
            }
            //rotate the vector to correct rotation
            tempOrign = rotationFreaze.transform.TransformDirection(tempOrign);
            tempDirection = rotationFreaze.transform.TransformDirection(tempDirection);
            Debug.DrawRay(tempOrign, tempDirection + (rotationFreaze.transform.TransformDirection(Vector3.forward) * 1000), Color.white);
            Debug.Log("Complete Calactulation of Ray");
            if (firstRay)
            {
                corner1 = new Ray(tempOrign, tempDirection);
            }
            else
            {
                corner2 = new Ray(tempOrign, tempDirection);
            }
        }
    }

    #endregion
}