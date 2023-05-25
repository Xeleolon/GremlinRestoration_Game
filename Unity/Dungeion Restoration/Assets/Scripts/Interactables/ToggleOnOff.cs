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
            PlayerChat.instance.NewMessage(message);
        }
        else if (toggleOff && !toggleObject.activeSelf)
        {
            toggleObject.SetActive(true);
            PlayerChat.instance.NewMessage(message);
        }
    }
}
