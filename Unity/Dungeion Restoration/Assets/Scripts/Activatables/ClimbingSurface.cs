using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingSurface : Activatable
{
    private PlayerMovements playerScript;
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovements>();
    }
    public override void Activate()
    {
        playerScript.ClimbingOn(true);
    }
    public override void UnActivate()
    {
        playerScript.ClimbingOn(false);
    }
}
