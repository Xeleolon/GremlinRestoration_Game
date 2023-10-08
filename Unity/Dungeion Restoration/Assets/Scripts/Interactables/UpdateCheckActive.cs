using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCheckActive : Interactable
{
    [SerializeField] GameObject target;

    public override void Interact()
    {
        CheckActive targetScript = target.GetComponent<CheckActive>();
        if (targetScript != null)
        {
            targetScript.CheckStatesOfObject();
        }
    }

}
