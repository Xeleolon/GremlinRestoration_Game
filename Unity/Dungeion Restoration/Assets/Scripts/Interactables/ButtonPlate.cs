using UnityEngine;

public class ButtonPlate : Interactable
{
    [Tooltip("how far down before activating")]
    [SerializeField] private float pressureDistance = 1;
    [SerializeField] private float returnSpeed = 1;
    [Tooltip("gameObject being activated by the trap")]
    public GameObject trapObject;
    [Tooltip("can the button once pressed again disable the trap when pressed again")]
    [SerializeField] private bool disableTrap = false;
    Vector3 startPosition;
    bool collisionActive = false; //is they a collision currently
    int numCollision = 0;
    bool trapActivated = false; //let the trap reset before activating again
    Rigidbody rb;
    public override void Interact()
    {
        string message = new string("It a Trap!");
        Debug.Log(message);
        PlayerChat.instance.NewMessage(message);
    }
    void Activate()
    {
        trapObject.GetComponent<Activatable>().OnActivate(disableTrap);
    }
    void Start()
    {
        startPosition = transform.position;
        //collisionActive = true;
        rb = GetComponent<Rigidbody>();
        if (!rb.isKinematic)
        {
            rb.isKinematic = true;
        }
        //Debug.Log(startPosition);
        //Debug.Log(transform.position);
        //Vector3 distance = startPosition - transform.position;
        //Debug.Log(distance);
    }

    
    void Update()
    {
        float distance = Vector3.Distance(startPosition, transform.position);
        if (collisionActive)
        {
        
            if (distance <= -pressureDistance || distance >= pressureDistance)
            {
                rb.isKinematic = true;
                if (!trapActivated)
                {
                    Debug.Log("Trap activated");
                Activate();
                trapActivated = true;
                }
                //Active Button!!
            }
        }
        else if (distance != 0)
        {
            if (rb.isKinematic)
            {
                rb.isKinematic = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, startPosition, returnSpeed * Time.deltaTime);
        }
        else if (!rb.isKinematic)
        {
            rb.isKinematic = true;
            trapActivated = false;
        }

    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Movable")
        {
            numCollision += 1;
            if (!collisionActive)
            {
                collisionActive = true;
                if (rb.isKinematic)
                {
                    rb.isKinematic = false;
                }
            }
        }
    }
    void DestoryGameObject()
    {
        Destroy(gameObject);
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Movable")
        {
            numCollision -= 1;
            if (collisionActive && numCollision == 0)
            {
                collisionActive = false;
            }
        }
    }
}
