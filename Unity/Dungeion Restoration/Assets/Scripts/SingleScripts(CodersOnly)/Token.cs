using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Token : MonoBehaviour
{
    private Vector3 target;
    private bool canMove = false;
    [SerializeField] private float speed;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image itemImage2;
    [SerializeField] GameObject particalEffect;

    void Update()
    {
        if (canMove)
        {
            if (transform.position != target)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
            else
            {
                Debug.Log("finished moving at " + target);
                canMove = false;
            }
        }
    }

    public void SetTarget(Vector3 newTarget, Item item)
    {
        target = newTarget;
        canMove = true;
        Debug.Log(target + " " + canMove + " token stuff");
        if (item != null)
        {
            itemImage.sprite = item.icon;
            itemImage2.sprite = item.icon;
        }
    }

    void OnDestroy()
    {
        Instantiate(particalEffect, transform.position, Quaternion.identity);
    }
}
