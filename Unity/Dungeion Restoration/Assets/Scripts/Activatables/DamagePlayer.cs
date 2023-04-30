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
    void Update()
    {
        if (willKill)
        {
            if (deathTimer <= 0)
            {
                player.GetComponent<PlayerMovements>().KillPlayer();
                deathTimer = deathWait;
                wholeCheck = deathWait;
                willKill = false;
            }
            else
            {
                if (deathTimer >= wholeCheck - 0.1 & deathTimer <= wholeCheck + 0.1)
                {
                    Debug.Log(killMessage + " " + wholeCheck);
                    wholeCheck -= 1;
                    PlayerChat.instance.NewMessage(killMessage + " " + wholeCheck);
                }

                deathTimer -= 1 * Time.deltaTime;
                
            }
        }
    }
    public override void Activate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        if (!willKill)
        {
            deathTimer = deathWait;
            willKill = true;
            wholeCheck = deathWait;
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
