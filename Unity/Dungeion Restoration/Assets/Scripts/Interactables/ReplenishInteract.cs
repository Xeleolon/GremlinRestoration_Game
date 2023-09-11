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
    [SerializeField] private GameObject refillObject;
    [SerializeField] private string animationName;
    
    public override void Start()
    {
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
            SpawnEffect(false);
            PlayAnimator(animationName);
            if (refillObject != null && !refillObject.activeSelf)
            {
                refillObject.SetActive(true);
            }
            if (!MonsterRequest)
            {
                Inventory.instance.Remove(requiredItem);
            }
            string message = new string(gameObject.name + " is Replenished");
            Debug.Log(message);
            Dialogue dialogue = new Dialogue(gameObject.name, message, 0);
            DebugController.instance.AddLog(dialogue);
            interacted = true;
        }
        else
        {
            OrderMessage(FailedMessage);
            Debug.Log(gameObject.name + " TaskFailed");
        }
    }

}
