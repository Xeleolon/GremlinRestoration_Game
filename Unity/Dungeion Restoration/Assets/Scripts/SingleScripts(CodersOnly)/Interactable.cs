using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    [Header("Standard Interactable")]
    [Tooltip("Radius which player can interact with object")]
    public Vector3 radius = new Vector3(3, 3, 3);
    [Tooltip("the offset of the position of the interaction border")]
    public Vector3 offSet;
    [Tooltip("gameObject being activated on Interaction")]
    [SerializeField] private GameObject activatable;
    [SerializeField] private bool OnActivate = false;
    [SerializeField] private bool cycleActived = false;
    [SerializeField] private GameObject secondInteract;
    [HideInInspector] public int interactionType = 0;
    [HideInInspector] public int interactionState = 0;
    [HideInInspector] public bool interacted = false;
    Transform player;


    [Tooltip("place a door or gate wich has the doorController script this task will become a requirement before opening")]
    [SerializeField] private DoorController taskForDoor;
    [SerializeField] private bool noBarDoor = false;



    [Header("Particals")]
    [Tooltip("The Prefab with a partical effect or an object you wish to spawn in the partical place")]
    [SerializeField] private Vector3 particalSpawnOffset;
    [SerializeField] private bool debugParticalPosition;
    [SerializeField] private GameObject particalPrefab;
    [Tooltip("Effect associated with a dud use")]
    public GameObject particalDudPrefab;

    [SerializeField] private Animator animator;
    [SerializeField] private bool animatationStartOpen;
    public string animationTriggerName;
    public GameObject correctObject;

    
    private bool finishTask = false;
    
    Vector3 radiusHalf;
    private PlayerInputActions playerControls;
    [HideInInspector]public InputAction fire;

    void Awake()
    {
        playerControls = new PlayerInputActions();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (animationTriggerName != "")
        {
            animator.SetBool(animationTriggerName, animatationStartOpen);
        }
    }

    public virtual void OnEnable()
    {
        fire = playerControls.Player.Fire;
        fire.Enable();
    }

    void OnDisable()
    {
        fire.Disable();
    }

    public virtual void Start()
    {
        if (particalPrefab == null)
        {
            particalPrefab = LevelManager.instance.sharedPrefabs.successPE;
        }
        radiusHalf.x = radius.x/2;
        radiusHalf.y = radius.y/2;
        radiusHalf.z = radius.z/2;

        if (taskForDoor != null && !noBarDoor)
        {
            taskForDoor.AddTasks();
        }
    }
    public virtual void Interact ()
    {
        //this method is meant to be overwritten
        Debug.Log("Interacting with " + transform.name);
        Activate(OnActivate);
        if (cycleActived)
        {
            OnActivate = !OnActivate;
        }
    }

    public bool OnInteract(Transform playerTransform, int state)
    {
        if (!interacted)
        {
            player = playerTransform;
            interactionState = state;
            //float distance = Vector3.Distance(player.position, transform.position);
            Vector3 distanceGreater = transform.position + offSet + radiusHalf;
            Vector3 distnaceSmaller = transform.position + offSet - radiusHalf;
            if (player.position.x <= distanceGreater.x && player.position.x >= distnaceSmaller.x && 
                player.position.y <= distanceGreater.y && player.position.y >= distnaceSmaller.y && 
                player.position.z <= distanceGreater.z && player.position.z >= distnaceSmaller.z)
            {
                Debug.Log("INTERACT");
                Interact();
                return true;
            }
            else
            {
                if (interacted == true)
                {
                    Debug.Log(gameObject.name + " already interacted");
                }
                return false;
            }
        }

        Interactable secondInteractable = secondInteract.GetComponent<Interactable>();
        if (secondInteract != null && secondInteractable != null)
        {
            return secondInteractable.OnInteract(playerTransform, state);
        }
        return false;
    }

    public virtual void Activate(bool unActivate)
    {
        //Debug.Log(activatable);
        if (activatable != null)
        {
            activatable.GetComponent<Activatable>().OnActivate(unActivate);
        }
    }
    public virtual void PlayAnimator(string playAnimation)
    {
        if (playAnimation != "" && animator != null)
        {
            if (playAnimation.Contains("activate"))
            {
                animator.enabled = true;
            }
            else
            {
                animator.Play(playAnimation);
            }
        }
    }

    public void OrderMessage(int num)
    {
        if (num > 0)
        {
            DialogueManager.instance.PlayDialogue(num);
        }
    }

    public void SpawnEffect(bool dud)
    {
        Vector3 spawnPosition = transform.position + particalSpawnOffset;
        if (dud && particalDudPrefab != null)
        {
            Instantiate(particalDudPrefab, spawnPosition, Quaternion.identity);
        }
        else if (particalPrefab != null)
        {
            Instantiate(particalPrefab, spawnPosition, Quaternion.identity);

            //if succefull check door
            if (taskForDoor != null)
            {
                if (!finishTask)
                {
                    if (!noBarDoor)
                    {
                        taskForDoor.CheckTaskOff();
                        finishTask = true;
                    }
                    else
                    {
                        taskForDoor.AddTasks();
                    }
                }
            }
        }
    }

    public virtual void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireCube((transform.position + offSet), radius);

        if (debugParticalPosition)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((transform.position + particalSpawnOffset), new Vector3(0.2f, 0.2f, 0.2f));
        }
    }
}
