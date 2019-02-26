using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnitsScripts.Behaviour;

using PlayerScripts.UnitCommands;
using Utilities;
using ItemScript;

public class InventoryHandlers : MonoBehaviour
{
    public List<UnitInventoryBehavior> inventory;
    public ItemHolder itemHolder;
    public InventorySlotBehavior hoveringSlot;
    public InventorySlotBehavior clickedSlot;

    /// <summary>
    /// Swap Items when both Inventory slot is occupied
    /// </summary>
    /// <param name="thisInfo"> Clicked slot information, so it can be saved to a temp. variable.</param>
    public void SwapItems(ItemInformation thisInfo)
    {
        ItemInformation informationHolder = thisInfo;
        // Set Image of this slot on hovered image
        clickedSlot.SetImage(hoveringSlot.itemImage.sprite);
        // Set Image from Item holder
        hoveringSlot.SetImage(itemHolder.imageHolder.sprite);
        clickedSlot.TransferItemData(hoveringSlot.curItemInfo);
        hoveringSlot.TransferItemData(informationHolder);
        
    }
}
