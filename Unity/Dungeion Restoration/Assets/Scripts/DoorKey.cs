using UnityEngine;

public class DoorKey : Interactable
{
    public GameObject door;
    void Start()
    {
        if (door == null)
        {
            Debug.LogError(gameObject.name + "Doesn't have an assigned door");
        }
        //DoorLock correctGameObject = door.GetComponent<DoorLock>();
        if (door.GetComponent<DoorLock>() == null)
        {
            Debug.LogError(gameObject.name + " assigned door (" + door.name + ") doesn't contain a DoorLock Script check this object");
        }

        if (door.GetComponent<DoorLock>().key != false)
        {
            door.GetComponent<DoorLock>().key = false;
        }
    }

    public override void Interact()
    {
        base.Interact();
        door.GetComponent<DoorLock>().key = true;
        Debug.Log(door.name + " Unlocked");
        Destroy(gameObject);
    }
    
}
