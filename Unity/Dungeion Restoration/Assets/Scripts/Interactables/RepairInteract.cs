using UnityEngine;

public class RepairInteract : Interactable
{
    
    [Header("Repair")]
    public GameObject FixedObject;
    public override void Interact()
    {
        base.Interact();
        if (interactionState == 1)
        {
            PlayAnimator();
            Completed();
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
    public override void PlayAnimator()
    {
        base.PlayAnimator();
    }
    public override void Completed()
    {
        base.Completed();
    }

    public void RepairObject()
    {
        Instantiate(FixedObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}