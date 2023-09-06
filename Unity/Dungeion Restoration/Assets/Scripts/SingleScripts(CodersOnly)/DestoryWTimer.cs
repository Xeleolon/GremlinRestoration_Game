using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryWTimer : MonoBehaviour
{
    [SerializeField] private float clock = 2;
    
    void Update()
    {
        if (clock > 0)
        {
            clock -= 1 * Time.deltaTime;
        }
        else if (clock >= 0)
        {
            Destroy(gameObject);
        }
    }
}
