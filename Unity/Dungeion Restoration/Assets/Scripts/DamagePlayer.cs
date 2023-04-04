using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [Tooltip("on contact how long before the player shell die if they don't move")]
    [SerializeField] private int deathWait = 0;
    private float deathTimer;
    private float wholeCheck; //check if the timer has a whole number;
    private GameObject player;
    private bool willKill;
    void Update()
    {
        if (willKill)
        {
            if (deathTimer <= 0)
            {
                player.GetComponent<PlayerMovements>().KillPlayer();
            }
            else
            {
                if (deathTimer >= wholeCheck - 0.1 & deathTimer <= wholeCheck + 0.1)
                {
                    Debug.Log("Player will die in " + wholeCheck);
                    wholeCheck -= 1;
                    PlayerChat.instance.NewMessage("Player will die in " + wholeCheck);
                }

                deathTimer -= 1 * Time.deltaTime;
                
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !willKill)
        {
            player = other.gameObject;
            deathTimer = deathWait;
            willKill = true;
            wholeCheck = deathWait;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player" && willKill)
        {
            willKill = false;
            PlayerChat.instance.NewMessage("Player Recovered");
        }
    }
}
