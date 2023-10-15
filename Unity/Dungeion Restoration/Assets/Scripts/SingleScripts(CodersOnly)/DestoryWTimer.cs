using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryWTimer : MonoBehaviour
{
    [SerializeField] private float clock = 2;
    [SerializeField] private bool noDestoryChildren = false;
    
    void Update()
    {
        if (clock > 0)
        {
            clock -= 1 * Time.deltaTime;
        }
        else if (clock <= 0)
        {
            if (noDestoryChildren)
            {
                transform.DetachChildren();
            }
            Destroy(gameObject);
        }
    }
}
