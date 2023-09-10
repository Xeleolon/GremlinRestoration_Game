using UnityEngine;

public class DestroyInteract : Interactable
{
    [Header("Destroy")]
    [SerializeField] private float destroyDelay = 0;
    [Tooltip("doesn't require an item to be pickup, fill in only with item required to pick up")]
    [SerializeField] private Item itemPickup;
    private ButtonPlate pressurePlate;
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
            //PlayAnimator();
            Completed();
            FinishTask();
            SpawnEffect(false);
            Activate(false);
            if (pressurePlate != null)
            {
                pressurePlate.ForceExit(gameObject);
            }
            string message = new string(gameObject.name + " Destoryed");
            Debug.Log(message);
            Dialogue dialogue = new Dialogue(gameObject.name, message, 0);
            DebugController.instance.AddLog(dialogue);
            interacted = true;
            if (itemPickup != null)
            {
                Inventory.instance.Add(itemPickup);
            }
            Destroy(gameObject, destroyDelay);
        }
        else
        {
            string message = new string("Beep Boop Bop, wrong mode selecting");
            SpawnEffect(true);
            Debug.Log(message);
            Dialogue dialogue = new Dialogue(gameObject.name, message, 0);
            DebugController.instance.AddLog(dialogue);
        }
    }
    
    public override void Completed()
    {
        base.Completed();
    }
    void OnCollisionExit(Collision other)
    {
        pressurePlate = other.gameObject.GetComponent<ButtonPlate>();
        if (pressurePlate != null)
        {
            pressurePlate = null;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        pressurePlate = other.gameObject.GetComponent<ButtonPlate>();
    }

}
