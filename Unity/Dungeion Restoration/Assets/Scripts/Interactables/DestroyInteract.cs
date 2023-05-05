using UnityEngine;

public class DestroyInteract : Interactable
{
    [Header("Destroy")]
    public float destroyDelay = 0;
    public override void Start()
    {
        acheiveGoal.type = 2;
        base.Start();
    }
    public override void Interact()
    {
        base.Interact();
        if (interactionState == 2)
        {
            PlayAnimator();
            Completed();
            string message = new string(gameObject.name + " Destoryed");
            Debug.Log(message);
            PlayerChat.instance.NewMessage(message);
            interacted = true;
            Destroy(gameObject, destroyDelay);
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
    public override void Completed()
    {
        base.Completed();
    }

}
