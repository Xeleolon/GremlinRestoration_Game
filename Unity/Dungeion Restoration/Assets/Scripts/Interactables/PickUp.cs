using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    [SerializeField] private Item item;
    private bool pickedUp = false;
    [SerializeField] private string animationName;
    [SerializeField] private GameObject removeObject;
    [SerializeField] private GameObject replaceObject;

    public override void Interact()
    {
        base.Interact();
        if (!pickedUp && item != null && Inventory.instance.Add(item))
        {
            Debug.Log("picking up " + item.name);
            Dialogue dialogue = new Dialogue(gameObject.name, "picking up " + item.name, 0);
            DebugController.instance.AddLog(dialogue);
            SpawnEffect(true);
            PlayAnimator(animationName);
            FinishTask();
            pickedUp = true;
            if (replaceObject != null && !replaceObject.activeSelf)
            {
                replaceObject.SetActive(true);
            }

            if (removeObject != null && removeObject.activeSelf)
            {
                removeObject.SetActive(false);
            }
        }
    }
}
