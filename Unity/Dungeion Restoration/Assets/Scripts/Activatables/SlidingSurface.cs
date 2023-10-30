using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingSurface : Activatable
{
    // Start is called before the first frame update
    PlayerMovements playerScript;

    public override void Activate()
    {
        if (playerScript == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            playerScript = player.GetComponent<PlayerMovements>();
        }
        playerScript.Sliding(true);
    }
    public override void UnActivate()
    {
       if (playerScript == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            playerScript = player.GetComponent<PlayerMovements>();
        }
        playerScript.Sliding(false);
    }
}
