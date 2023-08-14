using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    #region Singleton
    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than one instance of Inventory found!");
        }
        instance = this;
    }
    #endregion
    
    
    [Range(0,20)]
    [SerializeField] private int space = 20;
    [SerializeField] private bool infiniteItems = false;

    [SerializeField] private List<Item> items = new List<Item>();

    public delegate void OnItemChanged(); //allow other script to subscribe to this function and be informed off changes.
    public OnItemChanged onItemChangedCallback;

    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            if (!infiniteItems)
            {
                Debug.Log("Not enough room.");
                return false;
            }
            else
            {
                Debug.Log("Not stored, But Infinity ON");
                return true;
            }
        }
        
        items.Add(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke(); //lets other scripts know the been a change
        }

        return true;
    }
    public bool CheckAvalability(Item item)
    {
        if (item == null || infiniteItems || items.Contains(item))
        {
            return true;
        }

        return false;
    }

    public bool Remove(Item item)
    {
        if (infiniteItems || item == null)
        {
            return true;
        }

        if (items.Contains(item))
        {
            if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        Debug.Log("Removing " + item.name);
        items.Remove(item);
        return true;
        }
        return false;
    }

}
