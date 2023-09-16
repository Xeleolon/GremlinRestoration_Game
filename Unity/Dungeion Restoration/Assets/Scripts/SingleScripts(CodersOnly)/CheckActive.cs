using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckActive : MonoBehaviour
{
    [System.Serializable]
    public class ActiveObject
    {
        public Transform parent;
        public GameObject prefabTarget;
        public bool lastCheckstate;
    }

    private ActiveObject[] activeObjects;
    [SerializeField] private GameObject activableObject;
    [SerializeField] private Transform parentContainter;
    private int numberOfActive = 0; //keeps track of how many contanter are active
    private bool allActive;
    void Start()
    {
        if (parentContainter.childCount > 0)
        {
            activeObjects = new ActiveObject[parentContainter.childCount];

            for (int i = 0; i < parentContainter.childCount; i++)
            {
                Transform childObject = parentContainter.GetChild(i);
                Interactable childScript = childObject.gameObject.GetComponent<Interactable>();
                if (childScript == null && childObject.childCount > 0)
                {
                    childScript = childObject.GetChild(0).gameObject.GetComponent<Interactable>();
                }
                Debug.Log(i + "  " + childScript);
                if (childScript != null && childScript.correctObject != null)
                {
                    activeObjects[i].parent = childObject;
                    activeObjects[i].prefabTarget = childScript.correctObject;
                }
            }
        }
    }


    public void CheckStatesOfObject()
    {
        if (!allActive)
        {
            foreach(ActiveObject currentObject in activeObjects)
            {
                if (currentObject.parent.childCount > 0)
                {
                    Transform child = currentObject.parent.Find(currentObject.prefabTarget.name);
                    if (child != null)
                    {
                        if (!currentObject.lastCheckstate)
                        {
                            numberOfActive += 1;
                            currentObject.lastCheckstate = true;
                        }
                    }
                    else
                    {
                        if (currentObject.lastCheckstate)
                        {
                            numberOfActive -= 1;
                            currentObject.lastCheckstate = false;
                        }
                    }
                }
            }
    
            if (numberOfActive >= activeObjects.Length)
            {
                Debug.Log("Activing Something");
                allActive = true;
                Activatable checkScript = activableObject.GetComponent<Activatable>();
                if (checkScript != null)
                {
                    checkScript.Activate();
                }
            }
        }
    }
}
