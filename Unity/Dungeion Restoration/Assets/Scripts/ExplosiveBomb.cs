using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBomb : MonoBehaviour
{
    private string breakabletag = new string("Breakable");
    [SerializeField] private float radius = 3;
    [SerializeField] private float life = 3;
    [SerializeField] private GameObject particalEffect;
    [Tooltip("how powerful the force of the kick")]
    [SerializeField] private float playerKick = 500;
    [Tooltip("how much of the force is upwards")]
    [SerializeField] private float PlayerLiftKick = 2;
    private float currentLife;
    [SerializeField] private bool pausedCount;
    
    void Start()
    {
        currentLife = life;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pausedCount)
        {
            if (currentLife <= 0)
            {
                Explode();
            }
            else
            {
                currentLife -= 1 * Time.deltaTime;
            }
        }
    }

    void Explode()
    {
        if (particalEffect != null)
        {
            Instantiate(particalEffect, transform.position, Quaternion.identity);
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider hit in hitColliders)
        {
            if (breakabletag != "" && hit.gameObject.tag == breakabletag)
            {
                DestroyInteract checkDestroy = hit.GetComponent<DestroyInteract>();
                Breakable checkBreakable = hit.GetComponent<Breakable>();
                if (checkDestroy != null)
                {
                    checkDestroy.OnInteract(hit.transform, 2);
                }
                else if (checkBreakable != null)
                {
                    checkBreakable.Break();
                }
                else
                {
                    Destroy(hit.gameObject);
                }
            }

            if (hit.gameObject.tag == "Player")
            {
                //Debug.Log("Hit Player");
                //Apply force in reverse
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    Debug.Log("Hit Player");
                    rb.AddExplosionForce(playerKick, transform.position, radius, PlayerLiftKick);
                }
            }
            
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
