using UnityEngine;

public class DoorLock : Interactable
{
    [Header("DoorLock")]
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
            string message = new string("Player Requires " + keyname);
            Debug.Log(message);
            
            PlayerChat.instance.NewMessage(message);
        }
    }

    void OpenDoor()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        if (keyname != "")
        {
            string message = new string("Player Opened Door with " + keyname);
            Debug.Log(message);
            PlayerChat.instance.NewMessage(message);
        }
    }
}
