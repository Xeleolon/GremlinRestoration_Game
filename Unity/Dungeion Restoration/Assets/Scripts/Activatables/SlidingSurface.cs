using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingSurface : Activatable
{
    public bool slideOn = false; //tell the surface if it on or off


    public override void Activate ()
    {
        Debug.Log(gameObject.name + "now Sliding Surface");
        slideOn = true;
    }
    public override void UnActivate ()
    {
        Debug.Log(gameObject.name + "now Standard Surface");
        slideOn = false;
    }
}
