using UnityEngine;

public class RepairInteract : Interactable
{
    
    [Header("Repair")]
    public GameObject fixedModel;
    [SerializeField] private bool removeModel;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private Vector3 spawnRotation;
    [SerializeField] private bool UseParentRotation;
    [SerializeField] private bool destroyMode;
    [Tooltip("place Required Item to fix, leave blank if no item required to fix")]
    [SerializeField] private Item requiredItem;
    [SerializeField] private float holdInteractFor = 1;
    [SerializeField] private int failedMessage = 0;

    private bool interactHold;
    private float holdClock;


    public override void OnEnable()
    {
        base.OnEnable();
        if (interacted)
        {
            interacted = false;
        }
        if (particalDudPrefab == null)
        {
            particalDudPrefab = LevelManager.instance.sharedPrefabs.repairFailedPE;
        }
    }
    public override void Start()
    {
        interactionType = 1;
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
            SpawnEffect(true);
            SpawnToken(requiredItem);
            //Debug.Log(message);
            OrderMessage(failedMessage);
            Dialogue dialogue = new Dialogue(gameObject.name, message, 0);
            DebugController.instance.AddLog(dialogue);
            
        }
    }


    private bool Repair(int interactionState)
    {
        if (interactionState == 1 && Inventory.instance.Remove(requiredItem))
        {
            //PlayAnimator();
            SpawnEffect(false);
            FinishTask();
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
        if (destroyMode)
        {
            if (fixedModel != null)
            {
                if (!UseParentRotation)
                {
                    Quaternion objectRotation = Quaternion.identity;
                    objectRotation.eulerAngles = spawnRotation;
                    Instantiate(fixedModel, transform.position + spawnOffset, objectRotation);
                }
                else
                {
                    Instantiate(fixedModel, transform.parent);
                }
            }

            if (removeModel)
            {
                Destroy(gameObject);
            }
        }
        else
        {
        if (fixedModel != null && !fixedModel.activeSelf)
        {
            fixedModel.SetActive(true);
            
        }

        if (removeModel)
        {
            gameObject.SetActive(false);
        }
        }
    }

}