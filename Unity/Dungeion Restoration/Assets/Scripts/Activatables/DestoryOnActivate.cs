using UnityEngine;

public class DestoryOnActivate : Activatable
{
    public override void Activate ()
    {
        Debug.Log("Destroying " + gameObject.name);
        Destroy(gameObject);
    }
}
