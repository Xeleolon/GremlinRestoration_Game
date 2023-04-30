using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float radius = 3f;
    [Tooltip("gameObject being activated Interaction")]
    public GameObject trapObject;
    //[Tooltip("can the button once pressed again disable the trap when pressed again")]
    //[SerializeField] private bool disableTrap = false;
    //[Tooltip("0 for neutral, 1 for repair, 2 for destory, 3 for replinish")]
    //[Range(0, 3)]
    //public int interactionType = 0;
    public int interactionState = 0;
    public bool interacted = false;
    private Animator animator;
    public string activateAnimation;
    Transform player;
    public AcheiveGoal acheiveGoal;
    public virtual void Start()
    {
        if (acheiveGoal.goal != "")
        {
            acheiveGoal.ticket = GoalTracker.instance.CreateGoalData(acheiveGoal.goal);
        }
        else if (acheiveGoal.type != 0)
        {
            acheiveGoal.ticket = GoalTracker.instance.CreateGoalData(acheiveGoal.type.ToString());
        }
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
    public virtual void UnActivate()
    {
        trapObject.GetComponent<Activatable>().OnActivate(true);
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
        GoalTracker.instance.CompletedGoal(acheiveGoal.ticket);
    }
    public bool OnInteract (Transform playerTransform, int state)
    {
        player = playerTransform;
        interactionState = state;
        float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius && interacted != true) 
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
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
