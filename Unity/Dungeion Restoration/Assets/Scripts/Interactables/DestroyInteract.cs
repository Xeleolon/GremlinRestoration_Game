using UnityEngine;

public class DestroyInteract : Interactable
{
    public override void Interact()
    {
        base.Interact();
        if (interactionState == 2)
        {
            PlayAnimator();
            string message = new string(gameObject.name + " Destoryed");
            Debug.Log(message);
            PlayerChat.instance.NewMessage(message);
            interacted = true;
            Destroy(gameObject);
        }
        else
        {
            string message = new string("Beep Boop Bop");
            Debug.Log(message);
            PlayerChat.instance.NewMessage(message);
        }
    }
    public override void PlayAnimator()
    {
        base.PlayAnimator();
    }

}
