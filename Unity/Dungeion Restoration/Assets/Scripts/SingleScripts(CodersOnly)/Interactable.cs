using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    [System.Serializable]
    public class AcheiveGoal
    {
        public string goal;
        public int ticket = -1; //a log of the location inside goalTracker for the location of the item.
        [Range(0, 3)][Tooltip("0 if no string named no goal will be made, 1,2,3 will make a basic goal if no string named")]
        public int type = 0;
    }
    [Header("Standard Interactable")]
    [Tooltip("Radius which player can interact with object")]
    public Vector3 radius = new Vector3(3, 3, 3);
    [Tooltip("the offset of the position of the interaction border")]
    public Vector3 offSet;
    [Tooltip("gameObject being activated Interaction")]
    public GameObject trapObject;
    public int interactionState = 0;
    public bool interacted = false;
    Transform player;


    [Tooltip("place a door or gate wich has the doorController script this task will become a requirement before opening")]
    [SerializeField] private DoorController taskForDoor;

    [Tooltip("upon Interaction order message number leave 0 if no message is to be sent")]
    [SerializeField] private int orderInteractionMessage = 0;


    [Header("Particals")]
    [Tooltip("The Prefab with a partical effect or an object you wish to spawn in the partical place")]
    [SerializeField] private Vector3 particalSpawnOffset;
    [SerializeField] private GameObject particalPrefab;
    [Tooltip("Effect associated with a dud use")]
    [SerializeField] private GameObject particalDudPrefab;

    [SerializeField] private Animator animator;
    [SerializeField] private bool animatationStartOpen;
    [SerializeField] private string animationTriggerName;

    
    private bool finishTask = false;
    public AcheiveGoal acheiveGoal;
    Vector3 radiusHalf;
    private bool goalUpdated = false;

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

        ;
        if (acheiveGoal.goal != "")
        {
            //Debug.Log("Using string");
            acheiveGoal.ticket = GoalTracker.instance.CreateGoalData(acheiveGoal.goal);
        }
        else if (acheiveGoal.type != 0)
        {
            //Debug.Log("using number");
            acheiveGoal.ticket = GoalTracker.instance.CreateGoalData(acheiveGoal.type.ToString());
        }

        radiusHalf.x = radius.x/2;
        radiusHalf.y = radius.y/2;
        radiusHalf.z = radius.z/2;

        if (taskForDoor != null)
        {
            taskForDoor.AddTasks();
        }
    }
    public virtual void Interact ()
    {
        //this method is meant to be overwritten
        Debug.Log("Interacting with " + transform.name);
        OrderMessage(orderInteractionMessage);
    }

    public void OrderMessage(int num)
    {
        if (num > 0)
        {
            DialogueManager.instance.PlayDialogue(num);
        }
    }
    public virtual void Activate(bool unActivate)
    {
        trapObject.GetComponent<Activatable>().OnActivate(unActivate);
    }
    public virtual void PlayAnimator(string playAnimation)
    {
        if (playAnimation != "" && animator != null)
        {
            animator.Play(playAnimation);
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
        }
    }
    public virtual void Completed()
    {
        //GoalTracker.instance.UpdateChecklist(type);
        if (!goalUpdated && acheiveGoal.ticket >= 0)
        {
            Debug.Log(gameObject.name + " updating goal");
            GoalTracker.instance.CompletedGoal(acheiveGoal.ticket);
            goalUpdated = true;
        }
    }
    public void FinishTask()
    {
        if (taskForDoor != null)
        {
            if (!finishTask)
            {
                taskForDoor.CheckTaskOff();
                finishTask = true;
            }
        }
    }
    public bool OnInteract (Transform playerTransform, int state)
    {
        player = playerTransform;
        interactionState = state;
        //float distance = Vector3.Distance(player.position, transform.position);
        Vector3 distanceGreater = transform.position + offSet + radiusHalf;
        Vector3 distnaceSmaller = transform.position + offSet - radiusHalf;
            if (interacted != true && 
            player.position.x <= distanceGreater.x && player.position.x >= distnaceSmaller.x && 
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

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireCube((transform.position + offSet), radius);
    }
}
