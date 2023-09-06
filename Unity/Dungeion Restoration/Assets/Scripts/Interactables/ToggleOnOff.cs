using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnOff : Interactable
{
    public GameObject toggleObject;
    public bool toggleOn = true;
    public bool toggleOff = true;
    public string message;
    public override void Interact()
    {
        base.Interact();
        if (toggleOn && toggleObject.activeSelf)
        {
            toggleObject.SetActive(false);
            Completed();
            
            Dialogue dialogue = new Dialogue(gameObject.name, message, 0);
            DebugController.instance.AddLog(dialogue);
        }
        else if (toggleOff && !toggleObject.activeSelf)
        {
            toggleObject.SetActive(true);
            Dialogue dialogue = new Dialogue(gameObject.name, message, 0);
            DebugController.instance.AddLog(dialogue);
        }
    }
}
