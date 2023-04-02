using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingSurface : Activatable
{
    public bool slideOn = false; //tell the surface if it on or off

    private Vector2 yHeight;
    private GameObject player;
    public Vector3 target;
    private Vector3 targetTransformed;
    public float slideSpeed;
    private PlayerMovements playerScript;

    void Start()
    {
        yHeight.x = transform.position.y + (transform.localScale.y/100/2);
        yHeight.y = transform.position.y - (transform.localScale.y/100/2);

        player = GameObject.FindWithTag("Player");
        targetTransformed = transform.position + target;
    }
    void Update()
    {
        if (slideOn)
        {
            Debug.Log("Player Moved");
            float step = slideSpeed * Time.deltaTime;
            player.GetComponent<PlayerMovements>().canInteract = false;
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetTransformed, step);
        }
    }


    public override void Activate ()
    {
        Debug.Log(gameObject.name + "now Sliding Surface");
        slideOn = true;
    }
    public override void UnActivate ()
    {
        Debug.Log(gameObject.name + "now Standard Surface");
        slideOn = false;
    }
}
