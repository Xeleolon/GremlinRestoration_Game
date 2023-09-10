using UnityEngine;

public class RepairInteract : Interactable
{
    
    [Header("Repair")]
    public GameObject FixedModel;
    [SerializeField] private bool RemoveModel;
    [Tooltip("place Required Item to fix, leave blank if no item required to fix")]
    [SerializeField] private Item requiredItem;
    [SerializeField] private float holdInteractFor = 1;

    private bool interactHold;
    private float holdClock;


    public override void OnEnable()
    {
        base.OnEnable();
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
    void Update()
    {
        if (interactHold)
        { 
            float fireKey = fire.ReadValue<float>();

            if (fireKey > 0)
            {
                if (holdClock <= 0)
                {
                    Inventory.instance.Remove(requiredItem);
                    Completed();
                    RepairModel();
                    
                    string message = new string(gameObject.name + " Repaired");
                    Debug.Log(message);
                    Dialogue dialogue = new Dialogue(gameObject.name, message, 0);
                    DebugController.instance.AddLog(dialogue);
                    interacted = true;
                    interactHold = false;
                }
                else
                {
                    holdClock -= 1 * Time.deltaTime;
                }
            }
        }
    }
    public override void Interact()
    {
        base.Interact();
        if (!interactHold && interactionState == 1 && Inventory.instance.CheckAvalability(requiredItem)) //This is the initial interaction of the interact
        {
            //PlayAnimator();
            holdClock = holdInteractFor;
            interactHold = true;
        }

        if (!Repair(interactionState))
        {
            string message = new string("Beep Boop Bop");
            SpawnEffect(false);
            Debug.Log(message);
            Dialogue dialogue = new Dialogue(gameObject.name, message, 0);
            DebugController.instance.AddLog(dialogue);
            
        }
    }

    public override void Completed()
    {
        base.Completed();
    }

    private bool Repair(int interactionState)
    {
        if (interactionState == 1 && Inventory.instance.Remove(requiredItem))
        {
            //PlayAnimator();
            SpawnEffect(true);
            FinishTask();
            Completed();
            RepairModel();
            string message = new string(gameObject.name + " Repaired");
            Debug.Log(message);
            Dialogue dialogue = new Dialogue(gameObject.name, message, 0);
            DebugController.instance.AddLog(dialogue);
            interacted = true;
            return true;
        }

        return false;
    }

    public void RepairModel()
    {
        if (FixedModel != null)
        {
            FixedModel.SetActive(true);
            
        }

        if (RemoveModel)
        {
            gameObject.SetActive(false);
        }
    }

}