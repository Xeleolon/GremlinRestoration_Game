using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void ShootRay()
    {
        if (fireRay)
        {
            if (Input.GetAxisRaw("Fire1") != 0 || Input.GetButtonDown("Fire2"))
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
        else
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

    public void ChangeState()
    {
        if (Input.GetAxisRaw("InteractOne") != 0 && state != 1)
        {
            state = 1;
            //PlayerChat.instance.NewMessage(new string("interactionState has change to " + interactionState));
            NewState();
            //LevelManager.instance.ChangeInteractUI(state);
        }

        if (Input.GetAxisRaw("InteractTwo") != 0 && state != 2)
        {
            state = 2;
            NewState();
        }

        if (Input.GetAxisRaw("InteractThree") != 0 && state != 3)
        {
            state = 3;
            NewState();
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            tempState += (Input.GetAxisRaw("Mouse ScrollWheel") * scrollWheelSpeed);
            //Debug.Log("Dectecting ScrollWheel, which = " + tempState);
            if (tempState >= 1 || tempState <= -1)
            {
                state += (int) tempState;
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
    public void EnableCamera(Transform player)
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
    }
    public void MoveCamera(Transform player)
    {
        Vector2 cameraSpeed = GetMouseInput() * cameraSensitivity;

        // Calculate new rotation and store it for future changes
        cameraVelocity = new Vector2(
            Mathf.MoveTowards(cameraVelocity.x, cameraSpeed.x, cameraAcceleration.x * Time.deltaTime),
            Mathf.MoveTowards(cameraVelocity.y, cameraSpeed.y, cameraAcceleration.y * Time.deltaTime));
        
        cameraRotation += cameraVelocity * Time.deltaTime;

        cameraRotation.y = ClampCameraVerticalAngle(cameraRotation.y);

        // convert the camera rotation to euler angles 
        player.localEulerAngles = new Vector3(0, cameraRotation.x, 0);
        mainCamera.transform.localEulerAngles = new Vector3(cameraRotation.y, 0 ,0);
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
    private bool holdingJump = false; //is the player still holding jump when they land
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

    public CameraControls cameraControls;
    void OnValidate() //only calls if change when script reloads or change in value
    {
        interactions.WandsNotNull();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraControls.EnableCamera(transform);
        lastCheckPoint = transform.position;
        levelManager = LevelManager.instance;
        LevelManager.instance.ChangeInteractUI(interactions.state);
        interactions.WandsNotNull();
        rotationFreaze = new GameObject("DontDestoryPlayerRotation");
        //rotationFreaze.AddComponent<Rigidbody>();

    }
    void OnEnable()
    {
        cameraControls.EnableCamera(transform);
        
    }

    void Update()
    {
        if (interactActive)
        {
            if (!rotationFreazeMove)
            {
                MovementInputs();
            }
            else
            {
                SlowDown();
            }
        Moving(moveStateX);
        Moving(moveStateY);
        
        JumpFunction();
        interactions.ShootRay();
        interactions.ChangeState();


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
    }
    void FixedUpdate()
    {
        if (interactActive)
        {
            if (!rotationFreazeMove)
            {
                //Debug.Log(transform.TransformDirection(velocity) + " " + velocity);
                rb.MovePosition(rb.position + transform.TransformDirection(velocity) * Time.fixedDeltaTime);
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
        if (Input.GetAxisRaw("Vertical") > 0) //Y for Vertival movement
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
            if (Input.GetAxisRaw("Sprint") > 0)
            {
                moveStateX = 0;
            }
            else
            {
                float forwardAcceleration;
                if (veritcalAcceleration >= 1)
                {
                    forwardAcceleration = 1;
                }
                
                moveStateX = 1;
            }
        }
        else if (Input.GetAxisRaw("Vertical") < 0) //Y for Vertival movement
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
                forward = false;
            }
            else if (backward)
            {
                backward = false;
            }
            velocity.z = 0;
            moveStateX = 5;
            moveStateY = 5;
        }

        if (Input.GetAxisRaw("Horizontal") > 0) //X for Horizontal movement
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
        else if (Input.GetAxisRaw("Horizontal") < 0) //X for Horizontal movement
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
            }
            else if (left)
            {
                left = false;
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
    void OnTriggerEnter(Collider other)
    {
        if (!onGround && other.tag == groundTag)
        {
            if (Input.GetButton("Jump"))
            {
                holdingJump = true;
            }
            onGround = true;
            
            //rb.velocity = 0;
        }
        
        rotationFreazeMove = false;
    }
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("On Ground Stay Decatating " + other);
        if (!onGround && other.tag == groundTag && other.name != "Player")
        {
            onGround = true;
        }

        rotationFreazeMove = false;
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("trying to leave ground");
        if (onGround && other.tag == groundTag)
        {
            //Debug.Log("Leaving ground1");
            onGround = false;

        }
        rotationFreaze.transform.position = transform.position;
        rotationFreaze.transform.rotation = transform.rotation;
        rotationFreazeMove = true;
        //Debug.Log(gameObject.transform.rotation + " & rotationFreaze:" + rotationFreaze.transform.rotation);
    }
    void OnCollisionEnter(Collision other)
    {
        if (rotationFreazeMove)
        {
            Vector3 contactPoint = rotationFreaze.transform.TransformDirection(other.contacts[0].point);
            Debug.Log(contactPoint + " and unchange: " + other.contacts[0].point);
            //Debug.Log(contactPoint);
            Debug.Log(rotationFreaze.transform.position);
            if (forward && contactPoint.z > (rotationFreaze.transform.position.z - 0.1))
            {
                veritcalAcceleration = 0;
                Debug.Log("stoping forward momoent");
                Debug.Log(contactPoint.z);
                Debug.Log(rotationFreaze.transform.position.z - 0.1);
            }
            else if (backward && contactPoint.z < (rotationFreaze.transform.position.z + 0.1))
            {
                veritcalAcceleration = 1;
            }

            if (right && contactPoint.x > (rotationFreaze.transform.position.x - 0.1))
            {
                horizontalAcceleration = 0;
            }
            else if (left && contactPoint.x < (rotationFreaze.transform.position.x + 0.1))
            {
                horizontalAcceleration = 1;
            }
        }
    }
    #endregion
}