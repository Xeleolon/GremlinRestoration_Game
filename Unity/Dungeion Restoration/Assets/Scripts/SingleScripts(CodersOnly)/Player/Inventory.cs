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

    [HideInInspector] public Item[] items = new Item[4];
    [HideInInspector] public int[] itemNumber = new int[4];
    [SerializeField] Item key;
    [SerializeField] Item bomb;
    private int newItemPlace = -1;
    private bool addingKey = false;
    int numKeys;
    private bool addingBomb = false;
    [SerializeField] private int numBombs;
    [Range(1,20)]
    [SerializeField] private int maxBombs = 10;
    public bool infiniteItems;
    public delegate void OnItemChanged(); //allow other script to subscribe to this function and be informed off changes.
    public OnItemChanged onItemChangedCallback;

    [Header("UI")]
    [SerializeField] Transform itemsParent;
    [SerializeField] InventorySlot keySlot;
    InventorySlot[] slots;

    public void StartInventory()
    {
        LevelData levelData = gameObject.GetComponent<LevelManager>().levelData;
        numKeys = levelData.numKeys;
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = levelData.items[i];
            itemNumber[i] = levelData.itemNumber[i];
        }
        onItemChangedCallback += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        ItemChanged();
    }


    public bool Add(Item item)
    {
        if (item == key)
        {
            numKeys += 1;
            addingKey = true;
            ItemChanged();
            return true;
            
        }
        else if (item == bomb)
        {
            if (numBombs <= maxBombs)
            {
                numBombs += 1;
                addingBomb = true;
                ItemChanged();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            int place = Contains(item);
            if (place >= 0)
            {
                itemNumber[place] += 1;
                newItemPlace = place;
                ItemChanged();
                return true;
            }
            else
            {
                place = FindSpace();
                if (place >= 0)
                {
                    newItemPlace = place;
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
        if (item == key && numKeys > 0)
        {
            return true;
        }

        if (bomb == item && numBombs > 0)
        {
            return true;
        }
        if (item == null || infiniteItems)
        {
            return true;
        }
        int temp = Contains(item);
        if (temp >= 0 && itemNumber[temp] > 0)
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

        if (bomb == item && numBombs > 0)
        {
            numBombs -= 1;
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
        keySlot.AddItem(key, numKeys, addingKey);
        if (addingKey)
        {
            addingKey = !addingKey;
        }

        //update bomb num in UI here!
        if (addingBomb)
        {
            addingBomb = !addingBomb;
        }
        int usedItem = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            usedItem = checkNull(usedItem); //sets the start point at the same place as the first available empty item

            if (usedItem < items.Length && items[usedItem] != null)
            {
                bool playAnimation = false;
                if (newItemPlace >= 0 && newItemPlace == usedItem)
                {
                    playAnimation = true;
                    newItemPlace = -1;
                }
                slots[i].AddItem(items[usedItem], itemNumber[usedItem], playAnimation);
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
