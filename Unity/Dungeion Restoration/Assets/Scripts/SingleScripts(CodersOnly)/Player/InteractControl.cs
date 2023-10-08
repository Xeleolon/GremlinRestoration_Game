using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractControl : MonoBehaviour
{
    [System.Serializable]
    public class WandData
    {
        public string name;
        public GameObject wand;
        public string animation;
    }
    [Tooltip("1 for repair, 2 for destory")]
    [Range(0, 2)]
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
    [SerializeField] private GameObject bombModel;
    [SerializeField] Item bombItem;
    [SerializeField] private float spawnDistance;
    [HideInInspector] public bool hideDestory;
    public int toolMax = 2;
    public WandData[] wands = new WandData[4];
    [HideInInspector] public bool interactActive = true;
    private InputAction fire;
    private InputAction interactionOne;
    private InputAction interactionTwo;
    private InputAction interactionThree;
    private InputAction scrollwheel;
    private PlayerInputActions playerControls;
    void OnValidate() //only calls if change when script reloads or change in value
    {
        WandsNotNull();
    }
    void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    public void OnEnable()
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
    public void OnDisable()
    {
        fire.Disable();
        interactionOne.Disable();
        interactionTwo.Disable();
        interactionThree.Disable();
        scrollwheel.Disable();

    }
    void Start()
    {
        WandsNotNull();
        LevelManager.instance.ChangeInteractUI(state);
    }
    void Update()
    {
        UpdateInteractions();
        ScrollWheel();
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
    public void UpdateInteractions()
    {
        if (interactActive && !fireRay)
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
        //Debug.Log("Fire");
        if (interactActive && fireRay)
        {
                //Debug.Log("Fire2");
            Ray ray;
                
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (state != 3)
                {
                    Debug.Log("hit " + hit.collider.gameObject.name);
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
    
                    Dialogue dialogue = new Dialogue(hit.collider.gameObject.name, "hit with raycast interaction", 0);
                    DebugController.instance.AddLog(dialogue);
    
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
                            PlayAnimation(interactable.interactionType);
    
                            
                            fireRay = false;
                            if (centerSprite != null && centerSprite.activeSelf)
                            {
                                centerSprite.SetActive(false);
                            }
                            clock = refresh;
                        }
                    }
                }
                else if (bombModel != null && Inventory.instance.CheckAvalability(bombItem))
                {
                    float distance = Vector3.Distance(hit.collider.transform.position, transform.position);
                    Debug.Log("Spawn Bomb");
                    Vector3 spawnPoint = transform.TransformDirection((Vector3.forward * spawnDistance) + transform.position);
                    Instantiate(bombModel, transform.position + transform.forward*spawnDistance, Quaternion.identity);
                    Inventory.instance.Remove(bombItem);
                    /*if (spawnDistance <= distance)
                    {
                        //spawn bomb above this contact or forward of this position
                        
                    }
                    else
                    {
                        Instantiate(bombModel, transform.forward * spawnDistance, Quaternion.identity);
                    }*/
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
        if (!hideDestory)
        {
            state = 2;
            NewState();
        }
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
        if (interactActive && scrollwheelInput.y != 0)
        {
            tempState += (scrollwheelInput.y * scrollWheelSpeed);
            //Debug.Log("Dectecting ScrollWheel, which = " + tempState);
            if (tempState >= 1)
            {
                state += 1;
                if (state <= -1)
                {
                    state = toolMax;
                }
                else if (state >= toolMax + 1)
                {
                    state = 0;
                }
                tempState = 0;
                NewState();
            }
            else if (tempState <= -1)
            {
                state -= 1;
                if (state <= -1)
                {
                    state = toolMax;
                }
                else if (state >= toolMax + 1)
                {
                    state = 0;
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
            case 0:
            LevelManager.instance.ChangeInteractUI(state);
            if (wands[0].wand != null && wands[0].wand.activeSelf)
            {
                wands[0].wand.SetActive(true);
            }

            if (wands[1].wand != null && wands[1].wand.activeSelf)
            {
                wands[1].wand.SetActive(false);
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
            case 1:
            LevelManager.instance.ChangeInteractUI(state);

            if (wands[0].wand != null && wands[0].wand.activeSelf)
            {
                wands[0].wand.SetActive(false);
            }
            
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

            if (wands[0].wand != null && wands[0].wand.activeSelf)
            {
                wands[0].wand.SetActive(false);
            }

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

            if (wands[0].wand != null && wands[0].wand.activeSelf)
            {
                wands[0].wand.SetActive(false);
            }

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
