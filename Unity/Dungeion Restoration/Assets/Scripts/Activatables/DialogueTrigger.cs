using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Activatable
{
    public bool onStart = false;
    public Dialogue dialogue;
    public void Start()
    {
        if (onStart)
        {
            Activate();
        }
    }
    public override void Activate()
    {
        DialogueManager.instance.StartDialogue(dialogue);
        gameObject.SetActive(false);
    }
}
