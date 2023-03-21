using UnityEngine;

public class DoorLock : Interactable
{
    public bool key = false;
    public string keyname;

    public override void Interact()
    {
        base.Interact();
        if (key)
        {
        OpenDoor();
        }
        else if (keyname != "")
        {
            Debug.Log("Player Requires " + keyname);
        }
    }

    void OpenDoor()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        if (keyname != "")
        {
            Debug.Log("Player Opened Door with " + keyname);
        }
    }
}
