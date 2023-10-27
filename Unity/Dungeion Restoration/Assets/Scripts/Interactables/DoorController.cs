using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactable
{
    [Header("Door Controller")]
    [SerializeField] private bool doorOpen;
    [SerializeField] private bool activateOnInteract = true;
    [SerializeField] private GameObject bar;

    [SerializeField] private Item requireKey;
    private bool skeltonKey = false;
    private bool locked;

    private int taskCount;

    [Tooltip("Leave 0 if no message")]
    [SerializeField] private int messageTaskNotComplete = 0;
    [SerializeField] private int messageUnlocked = 0;
    [SerializeField] private int messageLocked = 2;




    [Tooltip("The Name of the animation being called")]
    [SerializeField] private string closingAnimation;
    [Tooltip("The Name of the animation being called")]
    [SerializeField] private string openingAnimation;

    public override void Start()
    {
        DebugController.instance.onSkeltonKeyCallback += SkeltonUnlock;
        base.Start();
        if (requireKey != null)
        {
            locked = true;
        }
        else
        {
            locked = false;
        }

        if (bar != null && taskCount <= 0 && bar.activeSelf)
        {
            bar.SetActive(false);
        }
    }
    public override void Interact()
    {
        if (skeltonKey || taskCount <= 0)
        {
            if (skeltonKey || activateOnInteract)
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
            SpawnEffect(true);
            OrderMessage(messageTaskNotComplete);
            Debug.Log("TaskNotComplete!");
            Dialogue dialogue = new Dialogue(gameObject.name, "Task not Complete!", 0);
            DebugController.instance.AddLog(dialogue);
        }
    }

    public void SwitchOpen()
    {
        if (skeltonKey || taskCount <= 0)
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
        else
        {
            SpawnEffect(true);
            OrderMessage(messageTaskNotComplete);
            Debug.Log("TaskNotComplete!");
            Dialogue dialogue = new Dialogue(gameObject.name, "Task not Complete!", 0);
            DebugController.instance.AddLog(dialogue);
        }
    }

    public void AddTasks()
    {
        taskCount += 1;
        if (bar != null && !bar.activeSelf)
        {
            bar.SetActive(true);
        }
    }

    public void CheckTaskOff()
    {
        taskCount -= 1;
        if (bar != null && taskCount <= 0 && bar.activeSelf)
        {
            bar.SetActive(false);
        }
    }

    private void OpenDoor()
    {
        if (skeltonKey || !locked)
        {
            doorOpen = true;
            Debug.Log("door Opening");
            Activate(OnActivate);
            PlayAnimator(openingAnimation);
        }
        else if (Inventory.instance.Remove(requireKey))
        {
            locked = false;
            OrderMessage(messageUnlocked);
            SpawnEffect(false);
            Debug.Log("door unlocked");
            Dialogue dialogue = new Dialogue(gameObject.name, "door unlocked", 0);
            DebugController.instance.AddLog(dialogue);
            OpenDoor();
        }
        else
        {
            SpawnEffect(true);
            OrderMessage(messageLocked);
            Debug.Log("Requrie Key");
            Dialogue dialogue = new Dialogue(gameObject.name, "Door Locked Require Keys", 0);
            DebugController.instance.AddLog(dialogue);
        }
    }

    private void CloseDoor()
    {
        doorOpen = false;
        PlayAnimator(closingAnimation);
    }
    private void SkeltonUnlock()
    {
        skeltonKey = !skeltonKey;
    }




    
}
