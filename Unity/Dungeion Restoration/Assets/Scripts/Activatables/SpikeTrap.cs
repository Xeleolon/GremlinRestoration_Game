using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Activatable
{
    public float distance = 3f;
    public float speed = 1f;
    public Transform spike;
    private Vector3 orgin;
    private bool spikeActack = false;
    void Start()
    {
        orgin = spike.position;
    }
    void Update()
    {
        if (!spikeActack && orgin != transform.position)
        {
            float step = speed * Time.deltaTime;
            spike.position = Vector3.MoveTowards(spike.position, orgin, step);
        }
        else if (spikeActack)
        {
            if (distance > 0 && spike.position.x <= orgin.x + distance)
            {
                float step = speed * Time.deltaTime;
                Vector3 target = new Vector3(orgin.x + distance, orgin.y, orgin.z);
                spike.position = Vector3.MoveTowards(spike.position, target, step);
            }
            else if (distance < 0 && spike.position.x >= orgin.x + distance)
            {
                float step = speed * Time.deltaTime;
                Vector3 target = new Vector3(orgin.x + distance, orgin.y, orgin.z);
                spike.position = Vector3.MoveTowards(spike.position, target, step);
            }
            
        }
    }
    public override void Activate ()
    {
       spikeActack = true;
    }
    public override void UnActivate()
    {
        spikeActack = false;   
    }
}
