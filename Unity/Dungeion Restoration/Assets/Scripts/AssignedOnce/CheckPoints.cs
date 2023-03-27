using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private Vector3 playerSize = new Vector3(0, 0, 0);
    private GameObject checkPointCollider;
    [SerializeField] private bool startCheckPoint;

    void Start()
    {
        if (transform.childCount == 1)
        {
            checkPointCollider = transform.GetChild(0).gameObject;
        }
        else
        {
            Debug.LogError(gameObject.name + " this checkPoint need onlyOne child with a collider of the checkpoint");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (playerSize.x == 0 || playerSize.y == 0 || playerSize.z == 0)
        {
           GameObject player = GameObject.FindWithTag("Player");
           if (player != null)
           {
            playerSize = transform.localScale;
           }
           else
           {
            Debug.LogWarning("Player Not Assigned Tag");
           }
        }
        Gizmos.color = Color.yellow;
        Vector3 wireFrame = new Vector3(transform.position.x, transform.position.y + (playerSize.y / 2));
        Gizmos.DrawWireCube(wireFrame, playerSize);
    }
}
