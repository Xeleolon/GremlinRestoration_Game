using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableActivatable : Activatable
{
    public GameObject turnOnOff;
    public bool turnOffSelf;
    public override void Activate()
    {
        if (turnOnOff != null && !turnOnOff.activeSelf)
        {
            turnOnOff.SetActive(true);
            if (turnOffSelf)
            {
                activated = false;
                gameObject.SetActive(false);
            }
        }
    }
     public override void UnActivate()
    {
        if (turnOnOff != null && turnOnOff.activeSelf)
        {
            turnOnOff.SetActive(false);
        }
    }

}
