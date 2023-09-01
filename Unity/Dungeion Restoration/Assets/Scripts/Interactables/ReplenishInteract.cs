using UnityEngine;
using UnityEngine.UI;

public class ReplenishInteract : Interactable
{
    [Header("Replinish Variables")]
    [Tooltip("place Required Item to Refill, leave blank if no item required to Refill")]
    [SerializeField] private Item requiredItem;

    [Tooltip("set true if this is requesting monster")]
    [SerializeField] private bool MonsterRequest;

    [SerializeField] private int FailedMessage = 0;
    
    public override void Start()
    {
        acheiveGoal.type = 3;
        base.Start();

        if (requiredItem == null)
        {
            Debug.LogWarning(gameObject.name + " No Required Item so no refill can be made");
        }

    }
    
    public override void Interact()
    {
        base.Interact();
        if (interactionState == 3 && requiredItem != null)
        {
            LevelManager.instance.OpenReplenishUI(requiredItem, this, MonsterRequest);
        }

    }

    

    public void Refill(bool success)
    {
        if (success)
        {
            FinishTask();
            Completed();
            string message = new string(gameObject.name + " is Replenished");
            Debug.Log(message);
            PlayerChat.instance.NewMessage(message);
            interacted = true;
        }
        else
        {
            OrderMessage(FailedMessage);
            Debug.Log(gameObject.name + " TaskFailed");
        }
    }
    
    public override void Completed() //old checking off for task list
    {
        base.Completed();
    }

}
