using UnityEngine;

public class ButtonPlate : Interactable
{
    [Header("ButtonPlate")]
    [Tooltip("how far down before activating")]
    [SerializeField] private float pressureDistance = 1;
    [SerializeField] private float returnSpeed = 1;
    public bool disableJump = false;
    Vector3 startPosition;
    bool collisionActive = false; //is they a collision currently
    
    int numCollision = 0;
    bool trapActivated = false; //let the trap reset before activating again
    PlayerMovements playerScript;
    Rigidbody rb;
    public override void OnEnable()
    {
        base.OnEnable();
        if (startPosition != Vector3.zero)
        {
            transform.position = startPosition;
            trapActivated = false;
            numCollision = 0;
            collisionActive = false;
        }
        else
        {
            startPosition = transform.position;
        }
    }
    public override void Interact()
    {
        string message = new string("acting trap!");
        Debug.Log(message);
        Dialogue dialogue = new Dialogue(gameObject.name, message, 0);
        DebugController.instance.AddLog(dialogue);
    }
    public override void Activate(bool unActivate)
    {
        base.Activate(unActivate);
    }
    public override void Start()
    {
        base.Start();
        
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
                    Activate(false);
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
            if (trapActivated)
            {
                if (disableJump && playerScript != null)
                {
                    playerScript.ForceOnGround(false);
                }
                Activate(true);
            }
            trapActivated = false;
        }

    }
    void OnCollisionEnter(Collision other)
    {
        Rigidbody rbCheck = other.gameObject.GetComponent<Rigidbody>();
        if (other.gameObject.tag == "Player")
        {
            playerScript = other.gameObject.GetComponent<PlayerMovements>();
        }
        //Debug.Log(rbCheck);
        if (rb != null)
        {
            //Debug.Log(other.gameObject.name + " moveing platform " + gameObject.name);
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
        if (other.gameObject.tag == "Player")
        {
            playerScript = null;
        }
        Rigidbody rbCheck = other.gameObject.GetComponent<Rigidbody>();
        if (rbCheck != null)
        {
            //Debug.Log(other.gameObject.name + " leaving platform " + gameObject.name);
            numCollision -= 1;
            if (collisionActive && numCollision == 0)
            {
                collisionActive = false;
            }
        }
    }
    public void ForceExit(GameObject other)
    {
        Rigidbody rbCheck = other.gameObject.GetComponent<Rigidbody>();
        if (rbCheck != null)
        {
            //Debug.Log(other.gameObject.name + " leaving platform " + gameObject.name);
            numCollision -= 1;
            if (collisionActive && numCollision == 0)
            {
                collisionActive = false;
            }
        }
    }
}
