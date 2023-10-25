using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    [Header("Pickup")]
    [SerializeField] private Item item;
    [Range(1,10)]
    [SerializeField] private int numPickUp = 1;
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
            //Debug.Log("picked up 1");
            if (numPickUp > 1)
            {
                for(int i = 1; i < numPickUp; i++)
                {
                    //Debug.Log("picked up " + (i + 1));
                    Inventory.instance.Add(item);
                }
            }


            Debug.Log("picking up " + item.name);
            Dialogue dialogue = new Dialogue(gameObject.name, "picking up " + item.name, 0);
            DebugController.instance.AddLog(dialogue);
            SpawnEffect(true);
            PlayAnimator(animationName);
            currentDelay = delayNextPickUp;
            pickedUp = true;

            Activate(false);


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
