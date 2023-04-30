using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    [Header("Standard Activatable")]
    public bool activated = false;
    public virtual void Activate ()
    {
        //this method is meant to be overwritten
        Debug.Log("Activating " + transform.name);
        activated = true;
    }
    public virtual void UnActivate()
    {
        Debug.Log("Disabling" + transform.name);
        activated = false;
    }
    public void OnActivate(bool disable)
    {
        if (!activated && !disable)
        {
            Activate();
            activated = true;
        }
        else if (activated && disable)
        {
            UnActivate();
            activated = false;
        }
    }


}
