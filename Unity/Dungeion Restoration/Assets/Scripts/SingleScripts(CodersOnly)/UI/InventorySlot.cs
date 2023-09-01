using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Item item; //this is the item within this slot or disired
    [SerializeField] private bool target; //is the object item a target or a container

    [SerializeField] private Image image;

    void Start()
    {
        if (item != null)
        {
            if (target && item.hiddenIcon != null)
            {
                image.sprite = item.hiddenIcon;
            }
            else if (item.icon != null)
            {
                image.sprite = item.icon;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (target)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            if (draggableItem.item == item)
            {
                draggableItem.parentAfterDrag = transform;
            }
        }
    }
    
}
