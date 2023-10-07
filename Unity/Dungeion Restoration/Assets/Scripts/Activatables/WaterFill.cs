using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFill : Activatable
{
    [Tooltip("first value min, second max set 0 for current position as max or minium hieght No negative values")]
    [SerializeField] private Vector2 waterHeight = Vector2.zero;
    private float startHeight;
    private bool negativeValue;
    [SerializeField] private float fillSpeed = 0.5f;
    [SerializeField] private bool fill;
    void Start()
    {
        startHeight = transform.position.y;
    }

    void Update()
    {
        if (fill && transform.position.y != startHeight + waterHeight.y)
        {
            Vector3 target = transform.position;
            target.y = startHeight + waterHeight.y;
            transform.position = Vector3.MoveTowards(transform.position, target, fillSpeed * Time.deltaTime);
        }
        else if (!fill && transform.position.y != startHeight - waterHeight.x)
        {
            Vector3 target = transform.position;
            target.y = startHeight - waterHeight.x;
            transform.position = Vector3.MoveTowards(transform.position, target, fillSpeed * Time.deltaTime);
        }
    }

    public override void Activate()
    {
        if (!fill)
        {
            fill = true;
        }
    }
    public override void UnActivate()
    {
        if (fill)
        {
            fill = false;
        }
    }
}
