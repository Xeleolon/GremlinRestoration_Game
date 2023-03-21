using UnityEngine;

public class ReplenishInteract : Interactable
{
    public override void Interact()
    {
        base.Interact();
        if (interactionState == 3)
        {
            string message = new string(gameObject.name + " Replenish it supples");
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
