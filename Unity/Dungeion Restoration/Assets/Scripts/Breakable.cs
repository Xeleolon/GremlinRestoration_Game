using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Range(1,3)]
    [Tooltip("1 to stop breaking, 2 for destroy after break, 3 for deactive")]
    [SerializeField] private int mode = 2;
    private bool disableBreak = false;
    [SerializeField] private bool active;
    [SerializeField] GameObject activatable;
    [SerializeField] bool asignItself = false;
    void OnValidate()
    {
        if (asignItself)
        {
            activatable = gameObject;
            asignItself = false;
        }
    }

    public void Break()
    {
        if (!disableBreak)
        {
            Debug.Log(activatable);
            if (activatable != null)
            {
                activatable.GetComponent<Activatable>().OnActivate(true);
            }
            
    
            switch (mode)
            {
                case 2:
                Destroy(gameObject);
                break;
                case 3:
                gameObject.SetActive(false);
                break;

                default:
                disableBreak = true;
                break;
            }
        }
    }
}
