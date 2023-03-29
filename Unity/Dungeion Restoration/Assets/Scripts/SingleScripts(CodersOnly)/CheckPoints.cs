using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private Vector3 playerSize = new Vector3(0, 0, 0);
    private Transform checkPointSpawn;
    [Tooltip("If Player Square is wrong just tick and the player square will asjust to the player current size")]
    [SerializeField] public bool updatePlayerSize = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetCheckPoint();
            other.GetComponent<PlayerMovements>().NewCheckPoint(checkPointSpawn.position);
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (updatePlayerSize)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerSize = player.transform.localScale;
                Debug.Log("playerSize Updated");
            }
            else
            {
                Debug.LogWarning("Player Not Assigned Tag");
            }

            if (updatePlayerSize)
            {
                updatePlayerSize = false;
            }
        }

        GetCheckPoint();

        Gizmos.color = Color.yellow;
        Vector3 wireFrame = new Vector3(checkPointSpawn.position.x, checkPointSpawn.position.y + (playerSize.y / 2), checkPointSpawn.position.z);
        Gizmos.DrawWireCube(wireFrame, playerSize);
    }
    void GetCheckPoint()
    {
    if (checkPointSpawn == null)
    {
        if (transform.childCount == 0)
        {
            //Debug.LogWarning(gameObject.name + " check Point is using it self as transform");
            checkPointSpawn = transform;
        }
        else
        {
            checkPointSpawn = transform.GetChild(0).gameObject.transform;
            
            if (transform.childCount > 1)
            {
                Debug.Log("CheckPoint has more than one child using this checkpoint: "+ checkPointSpawn.name);
            }
        }
    }
    }
}
