using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Item item; //this is the item within this slot or disired
    [SerializeField] private bool target; //is the object item a target or a container
    [Tooltip("Is this being used part of the Inventory systems")]
    [SerializeField] private bool inventory = false;
    [SerializeField] private Transform draggableParent;
    [SerializeField] private Image image;
    private GameObject pickUpObject;
    [SerializeField] private Image pickUpImage;
    private Animator pickUpAnimator;
    [SerializeField] private string ItemPickUpAnimation;
    [SerializeField] TMP_Text numText;
    [SerializeField] GameObject TextCanvas;

    void Start()
    {
        if (pickUpImage != null)
        {
            pickUpObject = pickUpImage.gameObject;
            pickUpAnimator = pickUpObject.GetComponent<Animator>();
        }
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

        if (draggableParent == null)
        {
            draggableParent = transform;
        }
    }

    public void UpdateTarget(Item targetItem)
    {
        item = targetItem;
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
                draggableItem.parentAfterDrag = draggableParent;

                LevelManager.instance.ReplenishReceipt(true);
                draggableItem.ReturnHome();
            }
        }
    }

    public void AddItem(Item newItem, int num)
    {
        if (image == null)
        {
            Debug.LogWarning(gameObject.name + " Does not have it image component assigned for the script!");
        }
        if (inventory && newItem != null)
        {
            if (item != newItem)
            {
                item = newItem;
                if (item.icon != null)
                {
                    Debug.Log("adding sprite");
                    image.sprite = item.icon;

                    if (pickUpImage != null)
                    {
                        if (!pickUpObject.activeSelf)
                        {
                            pickUpObject.SetActive(true);
                        }
                        pickUpImage.sprite = item.icon;
                        pickUpAnimator.Play(ItemPickUpAnimation);
                    }
                }
                else
                {
                    //Debug.Log("adding color");
                    image.color = item.tempColor;
                }
            }

            
            
            if (numText != null)
            {
                if (!TextCanvas.activeSelf)
                {
                    TextCanvas.SetActive(true);
                }
                numText.SetText(num.ToString());
            }
            image.enabled = true;
        }
    }

    public void ClearSlot()
    {
        if (inventory)
        {
            item = null;
            image.sprite = null;
            image.enabled = false;
            if (TextCanvas.activeSelf)
            {
                TextCanvas.SetActive(false);
            }
        }
    }
    public void UpdateNumOnly(int num)
    {
        if (numText != null)
        {
            if (num > 0)
            { 
                if (!TextCanvas.activeSelf)
                {
                    TextCanvas.SetActive(true);
                }
                numText.SetText(num.ToString());
            }
            else
            {
                if (TextCanvas.activeSelf)
                {
                    TextCanvas.SetActive(false);
                }
            }
        }
    }
    
}
