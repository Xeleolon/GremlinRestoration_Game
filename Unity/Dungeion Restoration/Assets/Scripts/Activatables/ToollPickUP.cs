using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToollPickUP : Activatable
{
    [SerializeField] int toolNumber;
    [SerializeField] bool destroy;
    [SerializeField] DoorController door;

    public override void Activate ()
    {
        LevelManager.instance.activeIcon(toolNumber);

        GameObject player = GameObject.FindWithTag("Player");
        InteractControl interactScript = player.GetComponent<InteractControl>();
        interactScript.ForceNewState(toolNumber);

        if (destroy)
        {
            Destroy(gameObject);
        }

        if (door != null)
        {
            door.SwitchOpen();
        }
    }
}
