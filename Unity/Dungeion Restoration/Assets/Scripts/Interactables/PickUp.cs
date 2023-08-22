using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    [SerializeField] private Item item;
    private bool pickedUp = false;

    public override void Interact()
    {
        base.Interact();
        if (!pickedUp && item != null && Inventory.instance.Add(item))
        {
            Debug.Log("picking up " + item.name);
            FinishTask();
            pickedUp = true;
        }
    }
}
