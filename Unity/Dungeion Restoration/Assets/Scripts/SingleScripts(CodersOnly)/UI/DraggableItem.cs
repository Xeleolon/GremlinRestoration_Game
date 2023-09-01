using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image image;

    [SerializeField] public Item item;

    [HideInInspector] public Transform parentAfterDrag;
    [SerializeField] private Canvas canvas;

    public void OnBeginDrag(PointerEventData eventData) 
    {
        
        if (item != null)
        {
            Debug.Log("Begin Drag");
    
            parentAfterDrag = transform.parent;
    
            transform.SetParent(transform.root);
    
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            Debug.Log("Dragging");
            
            PointerEventData pointerData = (PointerEventData)eventData;
    
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointerData.position, canvas.worldCamera, out position);
    
            transform.position = canvas.transform.TransformPoint(position);
       }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            Debug.Log("End Drag");
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
    }
    void Start()
    {
        if (item != null && item.icon != null)
        {
            image.sprite = item.icon;
        }
    }

    public void NewItem(Item newItem)
    {
        item = newItem;
        if (item.icon != null)
        {
            image.sprite = item.icon;
        }
    }
}
