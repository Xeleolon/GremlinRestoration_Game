using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : Interactable
{
    [Header("Collsion Conditions")]
    [SerializeField] string CheckTag;
    [SerializeField] bool trigger = false;
    [SerializeField] bool enter = false;
    [SerializeField] bool stay = false;
    [SerializeField] bool exit = false;
    public override void Activate()
    {
        base.Activate();
    }
    void CheckCollisionTag(GameObject other)
    {
        if (CheckTag != "")
            {
                if (other.tag == CheckTag)
                {
                    Activate();
                }
            }
            else
            {
                Activate();
            }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!trigger && enter)
        {
            CheckCollisionTag(other.gameObject);
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (!trigger && stay)
        {
            CheckCollisionTag(other.gameObject);
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (!trigger && exit)
        {
            CheckCollisionTag(other.gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (trigger && enter)
        {
           CheckCollisionTag(other.gameObject);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (trigger && stay)
        {
            CheckCollisionTag(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (trigger && exit)
        {
            CheckCollisionTag(other.gameObject);
        }
    }
}
