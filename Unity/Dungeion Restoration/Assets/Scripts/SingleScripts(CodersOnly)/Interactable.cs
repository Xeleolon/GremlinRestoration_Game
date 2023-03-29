using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Tooltip("Radius which player can interact with object")]
    public float radius = 3f;
    [Tooltip("gameObject being activated Interaction")]
    public GameObject trapObject;
    [Tooltip("can the button once pressed again disable the trap when pressed again")]
    [SerializeField] private bool disableTrap = false;
    //[Tooltip("0 for neutral, 1 for repair, 2 for destory, 3 for replinish")]
    //[Range(0, 3)]
    //public int interactionType = 0;
    public int interactionState = 0;
    public bool interacted = false;
    Transform player;
    public virtual void Interact ()
    {
        //this method is meant to be overwritten
        Debug.Log("Interacting with " + transform.name);
    }
    public virtual void Activate()
    {
        trapObject.GetComponent<Activatable>().OnActivate(disableTrap);
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
