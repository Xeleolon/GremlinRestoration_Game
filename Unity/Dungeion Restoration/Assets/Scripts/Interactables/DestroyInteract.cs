using UnityEngine;

public class DestroyInteract : Interactable
{
    [Header("Destroy")]
    [SerializeField] private float destroyDelay = 0;
    [Tooltip("doesn't require an item to be pickup, fill in only with item required to pick up")]
    [SerializeField] private Item itemPickup;
    [SerializeField] bool noDestroy = false;
    [SerializeField] private GameObject brokenVaraint;
    private ButtonPlate pressurePlate;
    [SerializeField] private bool useParent = false;
    public override void Start()
    {
        interactionType = 2;
        base.Start();
        if (particalDudPrefab == null)
        {
            particalDudPrefab = LevelManager.instance.sharedPrefabs.destoryFailedPE;
        }
    }
    public override void Interact()
    {
        if (interactionState == 2)
        {
            //PlayAnimator();
            SpawnEffect(false);
            PlayAnimator(animationTriggerName);
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
            if (!noDestroy)
            {
                if (brokenVaraint != null)
                { 
                    Instantiate(brokenVaraint, transform.parent);
                }
                if (useParent)
                {
                    Destroy(transform.parent.gameObject, destroyDelay);
                }
                else
                {
                    Destroy(gameObject, destroyDelay);
                }
            }
            else
            {
                if (brokenVaraint != null && !brokenVaraint.activeSelf)
                {
                brokenVaraint.SetActive(true);
                }
                //Debug.Log("Logging i just arrived");
                if (useParent)
                {
                    transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
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
