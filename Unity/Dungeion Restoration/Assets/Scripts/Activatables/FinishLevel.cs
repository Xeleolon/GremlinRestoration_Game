using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : Activatable
{
    public override void Activate()
    {
        LevelManager.instance.Victory();
        Debug.Log("Victory");
    }
}
