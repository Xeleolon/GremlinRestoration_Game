using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableActivatable : Activatable
{
    public GameObject turnOnOff;
    public bool reverseinputoutput = false;
    public bool turnOffSelf;
    public override void Activate()
    {
        if (!reverseinputoutput)
        {
            ShowObject();
        }
        else
        {
            HideObject();
        }
    }
     public override void UnActivate()
    {
        if (!reverseinputoutput)
        {
            HideObject();
        }
        else
        {
            ShowObject();
        }
    }

    void HideObject()
    {
        if (turnOnOff != null && turnOnOff.activeSelf)
        {
            turnOnOff.SetActive(false);
        }
    }

    void ShowObject()
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

}
