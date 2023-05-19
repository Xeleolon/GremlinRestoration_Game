using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : Activatable
{
    [Tooltip("on contact how long before the player shell die if they don't move")]
    [SerializeField] private int deathWait = 0;
    private float deathTimer;
    private float wholeCheck; //check if the timer has a whole number;
    private GameObject player;
    private bool willKill;
    public string killMessage = "Player will die in";
    void OnDisable()
    {
        deathTimer = deathWait;
        wholeCheck = deathWait;
        willKill = false;
    }
    void Update()
    {
        if (willKill)
        {
            if (deathTimer <= 0)
            {
                Debug.Log("Player Killed By " + gameObject.name);
                player.GetComponent<PlayerMovements>().KillPlayer();
                willKill = false;
                activated = false;
            }
            else
            {
                if (deathTimer >= wholeCheck - 0.1 & deathTimer <= wholeCheck + 0.1)
                {
                    //Debug.Log(killMessage + " " + wholeCheck);
                    PlayerChat.instance.NewMessage(killMessage + " " + wholeCheck);
                    wholeCheck -= 1;
                }

                deathTimer -= 1 * Time.deltaTime;
                
            }
        }
        else if (activated)
        {
            activated = false;
        }
    }
    public override void Activate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        PlayerMovements playerScript;
        playerScript = player.GetComponent<PlayerMovements>();
        if (!playerScript.CheckPlayerLife())
        {
            if (!willKill)
            {
                deathTimer = deathWait;
                wholeCheck = deathWait;
                willKill = true;
            }
        }
        else
        {
            willKill = false;
        }
    }
    public override void UnActivate()
    {
        if (willKill)
        {
            willKill = false;
            PlayerChat.instance.NewMessage("Player Recovered");
        }
    }
}
