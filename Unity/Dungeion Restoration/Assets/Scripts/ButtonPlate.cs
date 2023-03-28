using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlate : MonoBehaviour
{
    [Tooltip("how far down before activating")]
    [SerializeField] private float pressureDistance = 1;
    [SerializeField] private float returnSpeed = 1;
    Vector3 startPosition;
    bool collisionActive = false; //is they a collision currently
    int numCollision = 0;
    Rigidbody rb;
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
