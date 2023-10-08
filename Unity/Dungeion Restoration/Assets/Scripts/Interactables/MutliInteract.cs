using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutliInteract : Interactable
{
    private GameObject player;
    [SerializeField] private GameObject[] interactables;

    bool sequence;

    public override void Start()
    {
        base.Start();
    }
    public override void Interact()
    {
        
    }
}
