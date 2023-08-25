using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public RectTransform rect;
    [HideInInspector] public Transform parentAfterDrag;

    private PlayerInputActions playerControls;
    private InputAction look;
    void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    void OnEnable()
    {
        look = playerControls.Player.Look;
        look.Enable();
    }
    void OnDisable()
    {
        look.Disable();
    }
    public void OnBeginDrag(PointerEventData eventData) 
    {
        Debug.Log("Begin Drag");

        parentAfterDrag = transform.parent;

        transform.SetParent(transform.root);

        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        RectTransform rect = transform.root.GetComponent<RectTransform>();
        transform.position = Camera.main.ScreenToViewportPoint(look.ReadValue<Vector2>());
        //transform.position = RectTransformUtlility.ScreenPointToLocalPointInRectangle(rect, Camera.main);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
