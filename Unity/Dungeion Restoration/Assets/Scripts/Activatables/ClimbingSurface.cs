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
        if (playerScript == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            playerScript = player.GetComponent<PlayerMovements>();
        }
        playerScript.ClimbingOn(true);
    }
    public override void UnActivate()
    {
        if (playerScript == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            playerScript = player.GetComponent<PlayerMovements>();
        }
        playerScript.ClimbingOn(false);
    }
}
