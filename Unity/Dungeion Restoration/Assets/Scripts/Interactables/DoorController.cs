using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactable
{
    [Header("Door Controller")]
    [SerializeField] private bool doorOpen;
    [SerializeField] private bool activateOnInteract = true;


    [Tooltip("The Name of the animation being called")]
    [SerializeField] private string closingAnimation;
    [Tooltip("The Name of the animation being called")]
    [SerializeField] private string openingAnimation;

    public override void Start()
    {
        base.Start();
    }
    public override void Interact()
    {
        base.Interact();
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

    private void OpenDoor()
    {
        doorOpen = true;
        PlayAnimator(openingAnimation);
    }

    private void CloseDoor()
    {
        doorOpen = false;
        PlayAnimator(closingAnimation);
    }




    
}
