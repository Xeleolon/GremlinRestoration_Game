using UnityEngine;

public class DoorLock : Interactable
{
    [Header("DoorLock")]
    [Tooltip("set true if no key required")]
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
        else
        {
            string message = new string("Sorry, you need my key but I have no key");
            Debug.Log(message);
            
            PlayerChat.instance.NewMessage(message);
        }
    }

    void OpenDoor()
    {
        //need to come up with a solution for reenable collider
        Collider collider = GetComponent<Collider>(); 
        collider.enabled = false;
        if (keyname != "")
        {
            string message = new string("Player Opened Door with " + keyname);
            Debug.Log(message);
            PlayerChat.instance.NewMessage(message);
        }
        Destroy(gameObject);
    }
}
