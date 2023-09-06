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


    [Header("Inventory")]

    [SerializeField] public Item[] items = new Item[4];
     public int[] itemNumber = new int[4];
    [SerializeField] Item key;
    [SerializeField] int numKeys;
    [SerializeField] public bool infiniteItems;
    public delegate void OnItemChanged(); //allow other script to subscribe to this function and be informed off changes.
    public OnItemChanged onItemChangedCallback;

    [Header("UI")]
    [SerializeField] Transform itemsParent;
    [SerializeField] InventorySlot keySlot;
    InventorySlot[] slots;

    void Start()
    {
        onItemChangedCallback += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        ItemChanged();
    }


    public bool Add(Item item)
    {
        if (item == key)
        {
            numKeys += 1;
            ItemChanged();
            return true;
            
        }
        else
        {
            int place = Contains(item);
            if (place >= 0)
            {
                itemNumber[place] += 1;
                ItemChanged();
                return true;
            }
            else
            {
                place = FindSpace();
                if (place >= 0)
                {
                    Debug.Log("Adding " + item.name + "to Inventory");
                    //organise a new spot then
                    items[place] = item; //set item
                    itemNumber[place] += 1; //add 1 item
                    ItemChanged();
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckAvalability(Item item)
    {
        if (item == null || infiniteItems || key == item || Contains(item) >= 0)
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

        if (key == item && numKeys > 0)
        {
            numKeys -= 1;
            ItemChanged();
            return true;
        }

        
        int place = Contains(item);

        if (place >= 0)
        {
            if (itemNumber[place] > 1)
            {
                itemNumber[place] -= 1;
            }
            else
            {
                itemNumber[place] = 0;
                items[place] = null;
                
            }
            ItemChanged();
            return true;
        }


        

        return false;
    }

    private int Contains(Item item) //checks and find the location of the item in the inventory
    {
        if (item != null && items.Length > 0)
        {
            for (int i = 0; i < items.Length; i ++)
            {
                if (items[i] == item)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private int FindSpace()
    {
        if (items.Length > 0)
        {
            for (int i = 0; i < items.Length; i ++)
            {
                if (items[i] == null)
                {
                    return i;
                }
            }
        }
        Debug.LogWarning("Rang Out of Space");
        return 0;
    }

    private void ItemChanged()
    {
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    private void UpdateUI()
    {
        Debug.Log("Updating UI");
        keySlot.AddItem(key, numKeys);
        int usedItem = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            usedItem = checkNull(usedItem); //sets the start point at the same place as the first available empty item

            if (usedItem < items.Length && items[usedItem] != null)
            {
                slots[i].AddItem(items[usedItem], itemNumber[usedItem]);
                usedItem ++;
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public int checkNull(int startNumber)
    {
        if (startNumber < items.Length)
        {
            for (int i = startNumber; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    return i;
                }

            }
        }
        return items.Length;
    }

}
