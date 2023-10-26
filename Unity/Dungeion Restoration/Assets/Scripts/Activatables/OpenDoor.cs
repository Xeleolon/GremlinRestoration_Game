using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : Activatable
{
    [SerializeField] DoorController door;
    [Tooltip("The Name of the animation being called")]
    [SerializeField] private string closingAnimation;
    [SerializeField] private string openingAnimation;
    [SerializeField] private Animator animator;
    private bool open = true;

    public override void Activate()
    {
        if (door != null)
        {
            OpenClose();
        }
    }

    public override void UnActivate()
    {
        if (door != null)
        {
            OpenClose();
        }
    }

    private void OpenClose()
    {
        door.SwitchOpen();
        if (animator != null)
        {
            if (open && closingAnimation != "")
            {
                animator.Play(closingAnimation);
                open = false;
            }
            else if (!open && openingAnimation != "")
            {
                animator.Play(openingAnimation);
                open = true;
            }
        }
    }
}
