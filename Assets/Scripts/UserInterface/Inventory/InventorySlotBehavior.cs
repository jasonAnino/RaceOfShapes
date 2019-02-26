using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitsScripts.Behaviour;
using UnityEngine.EventSystems;

using PlayerScripts.UnitCommands;
using Utilities;
using ItemScript;
using TMPro;

public class InventorySlotBehavior : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnitInventoryBehavior localOwner;
    public Image itemImage;
    public bool clickedButton;
    public TextMeshProUGUI textHolder;
    [Header("SLOT RESTRICTIONS")]
    public bool occupied = false;
    public bool canPlaceAny = true;
    public List<ItemType> itemRestrictions;
    [Header("ITEM INFORMATION")]
    public ItemInformation curItemInfo;

    public void Start()
    {
        if(occupied)
        {
            if(textHolder != null)
            textHolder.text = "x" + curItemInfo.count;
        }
    }
    #region TransferData
    public void TransferItemData(BaseItemComponent thisItem)
    {
        curItemInfo = ItemInformation.DeepCopy(thisItem.itemInfo);
        textHolder.enabled = true;
        textHolder.text = "x" + curItemInfo.count;
    }
    public void TransferItemData(ItemInformation thisItem)
    {
        curItemInfo = ItemInformation.DeepCopy(thisItem);
        if (textHolder != null)
        {
            textHolder.enabled = true;
            textHolder.text = "x" + curItemInfo.count;
        }
    }

    public virtual void EquipThisItem()
    {

    }
    public void SetImage(Sprite newSprite)
    {
        occupied = true;
        itemImage.sprite = newSprite;
        itemImage.color = new Color(1, 1, 1, 1);
    }

    public void DisableImage()
    {
        itemImage.color = new Color(1, 1, 1, 0);
        occupied = false;
        textHolder.text = "";
        textHolder.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!occupied) return;
        if(ClickStateManager.GetInstance.currentState != ClickState.Idle)
        {
            return;
        }

        ClickStateManager.GetInstance.currentState = ClickState.UIClick;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!occupied) return;
            if(curItemInfo.isConsumable)
            {
                Debug.Log("USING ITEM : " + curItemInfo.itemName);
                curItemInfo.count -= 1;
                curItemInfo.UseItem(localOwner.owner);
                if(curItemInfo.count <= 0)
                {
                    curItemInfo = null;
                    DisableImage();
                    occupied = false;
                }
            }
        }
        else if(eventData.button == PointerEventData.InputButton.Left)
        {
            //Debug.Log("Get Item from : " + this.gameObject.name);
            //Debug.Log("Player clicked on Pointer Down");
            localOwner.inventoryManager.clickedSlot = this;
            localOwner.inventoryManager.itemHolder.CopyImageToholder(itemImage.sprite);
            localOwner.inventoryManager.itemHolder.StartFollowMouse();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (ClickStateManager.GetInstance.currentState != ClickState.UIClick)
            return;
            ClickStateManager.GetInstance.currentState = ClickState.Idle;

        if (localOwner.inventoryManager.itemHolder.followMouse)
        {
            if(localOwner.inventoryManager.hoveringSlot != null)
            {
                if(localOwner.inventoryManager.hoveringSlot != this)
                {
                    if(!localOwner.inventoryManager.hoveringSlot.AllowItemTransfer(curItemInfo.itemType))
                    {
                        Debug.Log("Transfer of Item to : " + localOwner.inventoryManager.hoveringSlot + " is invalid, check restrictions!");
                        localOwner.inventoryManager.itemHolder.StopFollowMouse();
                        return;
                    }
                    if(localOwner.inventoryManager.hoveringSlot.occupied)
                    {
                        localOwner.inventoryManager.SwapItems(curItemInfo);
                    }
                    else
                    {
                        localOwner.inventoryManager.clickedSlot = null;
                        localOwner.inventoryManager.hoveringSlot.SetImage(itemImage.sprite);
                        localOwner.inventoryManager.hoveringSlot.TransferItemData(curItemInfo);
                        curItemInfo = null;
                        DisableImage();
                        localOwner.inventoryManager.hoveringSlot = null;
                    }
                }
            }
        }
        localOwner.inventoryManager.itemHolder.StopFollowMouse();
        clickedButton = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(localOwner.inventoryManager.hoveringSlot != this)
        {
            localOwner.inventoryManager.hoveringSlot = this;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(localOwner.inventoryManager.hoveringSlot != null)
        {
            if(localOwner.inventoryManager.hoveringSlot == this)
            {
                localOwner.inventoryManager.hoveringSlot = null;
            }
        }
    }

    public bool AllowItemTransfer(ItemType thisType)
    {
        if (canPlaceAny) return true;
        else return itemRestrictions.Contains(thisType);

    }
    #endregion
}
