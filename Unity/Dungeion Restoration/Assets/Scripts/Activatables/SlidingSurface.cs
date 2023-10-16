using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingSurface : Activatable
{
    // Start is called before the first frame update
    PlayerMovements playerScript;
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovements>();
    }

    public override void Activate()
    {
        playerScript.Sliding(true);
    }
    public override void UnActivate()
    {
        playerScript.Sliding(false);
    }
}
