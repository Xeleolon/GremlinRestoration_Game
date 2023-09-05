using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAI : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent navMesh;


    [SerializeField] private Vector3[] target;
    
    [SerializeField] private Transform checkPoint; //Container Holding Checkpoints

    [SerializeField] private int targetPlace;

    [SerializeField] private bool completeRandom;

    private bool stopCycle = true;
    [SerializeField] private bool freaze = false;


    void Start()
    {
        navMesh = GetComponent<UnityEngine.AI.NavMeshAgent>();

        AssignCheckPoints();
    }

    void FixedUpdate()
    {
        if (target.Length <= 1)
        {
            stopCycle = true;
        }

        if (!stopCycle && !freaze)
        {
            if (transform.position == new Vector3(target[targetPlace].x, transform.position.y, target[targetPlace].z))
            {
                if (completeRandom)
                {
                    RandomisedCycle();
                }
                else
                {
                    CycleDestination();
                }
            }
            //navMesh.destination = target[0];
        }
    }

    void CycleDestination()
    {
        //Debug.Log("Cycling at " + targetPlace);
        if (targetPlace >= target.Length - 1)
        {
            targetPlace = 0;
        }
        else
        {
            targetPlace += 1;
        }

        navMesh.destination = target[targetPlace];
    }

    void RandomisedCycle()
    {
        //Debug.Log("Ramdomising at " + targetPlace);
        int newPlace = targetPlace;
        
        while(newPlace == targetPlace)
        {
            newPlace = Random.Range(0, (target.Length - 1));
        }

        targetPlace = newPlace;
        navMesh.destination = target[targetPlace];
    }

    void AssignCheckPoints()
    {

        int numOfTarget = checkPoint.childCount + 1;

        target = new Vector3[numOfTarget];

        target[0] = transform.position; //start postion is allway 1st

        for (int i = 1; i < numOfTarget; i++)
        {
            GameObject temp = checkPoint.GetChild(i - 1).gameObject;

            target[i] = temp.transform.position;

            Destroy(temp);
        }
        stopCycle = false;

        //navMesh.destination = target[targetPlace];



    }
}
