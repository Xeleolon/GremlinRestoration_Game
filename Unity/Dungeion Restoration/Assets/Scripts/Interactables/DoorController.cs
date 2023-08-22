using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactable
{
    [Header("Door Controller")]
    [SerializeField] private bool doorOpen;
    [SerializeField] private bool activateOnInteract = true;

    [SerializeField] private Item requireKey;
    private bool locked;

    private int taskCount;

    [Tooltip("Leave 0 if no message")]
    [SerializeField] private int messageTaskNotComplete = 0;
    [SerializeField] private int messageUnlocked = 0;
    [SerializeField] private int messagelocked = 0;




    [Tooltip("The Name of the animation being called")]
    [SerializeField] private string closingAnimation;
    [Tooltip("The Name of the animation being called")]
    [SerializeField] private string openingAnimation;

    public override void Start()
    {
        base.Start();
        if (requireKey != null)
        {
            locked = true;
        }
        else
        {
            locked = false;
        }
    }
    public override void Interact()
    {
        //base.Interact();
        if (taskCount <= 0)
        {
            if (activateOnInteract)
            {
                if (doorOpen)
                {
                    CloseDoor();
                }
                else
                {
                    OpenDoor();
                }
            }
        }
        else
        {
            OrderMessage(messageTaskNotComplete);
            Debug.Log("TaskNotComplete!");
        }
    }

    public void AddTasks()
    {
        taskCount += 1;
    }

    public void CheckTaskOff()
    {
        taskCount -= 1;
    }

    private void OpenDoor()
    {
        if (!locked)
        {
            doorOpen = true;
            PlayAnimator(openingAnimation);
        }
        else if (Inventory.instance.Remove(requireKey))
        {
            locked = false;
            OrderMessage(messageUnlocked);
            Debug.Log("door unlocked");
        }
        else
        {
            OrderMessage(messagelocked);
            Debug.Log("Requrie Key");
        }
    }

    private void CloseDoor()
    {
        doorOpen = false;
        PlayAnimator(closingAnimation);
    }




    
}
