using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    [SerializeField] private Item item;
    private bool pickedUp = false;
    [Tooltip("true if can pick object up in General use only for Bomb items")]
    [SerializeField] private bool rePickUp;
    [SerializeField] private float delayNextPickUp;
    private float currentDelay;
    [SerializeField] private string animationName;
    [SerializeField] private GameObject removeObject;
    [SerializeField] private GameObject replaceObject;

    void Update()
    {
        if (rePickUp && pickedUp)
        {
            if (currentDelay < 0)
            {
                pickedUp = false;
                
                if (replaceObject != null && replaceObject.activeSelf)
                {
                    replaceObject.SetActive(false);
                }
    
                if (removeObject != null && !removeObject.activeSelf)
                {
                    removeObject.SetActive(true);
                }
                
            }
            else
            {
                currentDelay -= 1 * Time.deltaTime;
            }
        }
    }
    public override void Interact()
    {
        if (!pickedUp && item != null && Inventory.instance.Add(item))
        {
            Debug.Log("picking up " + item.name);
            Dialogue dialogue = new Dialogue(gameObject.name, "picking up " + item.name, 0);
            DebugController.instance.AddLog(dialogue);
            SpawnEffect(true);
            PlayAnimator(animationName);
            currentDelay = delayNextPickUp;
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
