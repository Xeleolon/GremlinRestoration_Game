using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : Interactable
{
    [System.Serializable]
    public class collisionTypes
    {
        public bool enter = false;
        [Tooltip("true if upon entering you activate, false if your unactive")]
        public bool onEnter = true;
        public bool stay = false;
        [Tooltip("true if upon staying you activate, false if your unactive")]
        public bool onStay = true;
        public bool exit = false;
        [Tooltip("true if upon exiting you activate, false if your unactive")]
        public bool onExit = true;
    }
    [Header("Collsion Conditions")]
    [SerializeField] string checkTag;
    [SerializeField] bool trigger = false;
    [SerializeField] collisionTypes activate;
    public override void Start()
    {
        base.Start();
        //activate = new collisionTypes();
    }
    void CheckCollisionTag(GameObject other, bool onOff)
    {
        if (checkTag != "")
            {
                if (other.tag == checkTag)
                {
                    if (onOff)
                    {
                        Activate(false);
                    }
                    else
                    {
                        Activate(true);
                    }
                }
            }
            else
            {
                if (onOff)
                    {
                        Activate(false);
                    }
                    else
                    {
                        Activate(true);
                    }
            }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!trigger && activate.enter)
        {
                CheckCollisionTag(other.gameObject, activate.onEnter);
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (!trigger && activate.stay)
        {
            CheckCollisionTag(other.gameObject, activate.onStay);
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (!trigger && activate.exit)
        {
            CheckCollisionTag(other.gameObject, activate.onExit);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Test 1 " + trigger + activate.enter);
        if (trigger && activate.enter)
        {
            //Debug.Log("Test 2");
           CheckCollisionTag(other.gameObject, activate.onEnter);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (trigger && activate.stay)
        {
            CheckCollisionTag(other.gameObject, activate.onStay);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (trigger && activate.exit)
        {
            CheckCollisionTag(other.gameObject, activate.onExit);
        }
    }
}
