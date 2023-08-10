using UnityEngine;

public class RepairInteract : Interactable
{
    
    [Header("Repair")]
    public GameObject FixedModel;
    [Tooltip("place Required Item to fix, leave blank if no item required to fix")]
    [SerializeField] private Item requiredItem;
    void OnEnable()
    {
        if (interacted)
        {
            interacted = false;
        }
    }
    public override void Start()
    {
        acheiveGoal.type = 1;
        base.Start();
    }
    public override void Interact()
    {
        base.Interact();
        if (!Repair(interactionState))
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

    private bool Repair(int interactionState)
    {
        if (interactionState == 1 && Inventory.instance.Remove(requiredItem))
        {
            PlayAnimator();
            Completed();
            if (FixedModel != null)
            {
                RepairModel();
            }
            string message = new string(gameObject.name + " Repaired");
            Debug.Log(message);
            PlayerChat.instance.NewMessage(message);
            interacted = true;
            return true;
        }

        return false;
    }

    public void RepairModel()
    {
        FixedModel.SetActive(true);
        gameObject.SetActive(false);
    }

}