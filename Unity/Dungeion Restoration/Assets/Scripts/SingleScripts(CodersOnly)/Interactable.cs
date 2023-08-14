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
    private Animator animator;
    public string activateAnimation;
    Transform player;
    public AcheiveGoal acheiveGoal;
    Vector3 radiusHalf;
    private bool goalUpdated = false;

    private PlayerInputActions playerControls;
    public InputAction fire;

    void Awake()
    {
        playerControls = new PlayerInputActions();
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
    }
    public virtual void Interact ()
    {
        //this method is meant to be overwritten
        Debug.Log("Interacting with " + transform.name);
        animator = GetComponent<Animator>();
    }
    public virtual void Activate(bool unActivate)
    {
        trapObject.GetComponent<Activatable>().OnActivate(unActivate);
    }
    public virtual void PlayAnimator()
    {
        if (activateAnimation != "")
        {
            animator.Play(activateAnimation);
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
