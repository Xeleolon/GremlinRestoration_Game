using UnityEngine;

public class RepairInteract : Interactable
{
    public override void Interact()
    {
        base.Interact();
        if (interactionState == 1)
        {
            string message = new string(gameObject.name + " Repaired");
            Debug.Log(message);
            PlayerChat.instance.NewMessage(message);
            interacted = true;
        }
        else
        {
            string message = new string("Beep Boop Bop");
            Debug.Log(message);
            PlayerChat.instance.NewMessage(message);
        }
    }

}