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

    public LayoutSort sorter;
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
    
    /// <summary>
    /// Swaps The Inventory to be shown, usually adjusts depending on who's the manual unit controlled
    /// </summary>
    public void SwapUnitInventory(UnitBaseBehaviourComponent setThis)
    {
        if(inventory == null)
        {
            Debug.Log("Inv is null");
            return;
        }
        if(PlayerUnitController.GetInstance.manualControlledUnit == null)
        {
            Debug.Log("Manual is null");
            return;
        }
        if(inventory.Contains(inventory.Find(x => x.owner == setThis)))
        {
            Debug.Log("Its inside!");
            Transform inventoryTransform = inventory.Find(x => x.owner == setThis).transform;
            if(inventoryTransform != null)
            {
                Debug.Log("It can be seen");
                sorter.SetToFirst(inventoryTransform);
            }
        }
    }
}
