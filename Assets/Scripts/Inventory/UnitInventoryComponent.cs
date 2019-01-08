using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ComboSystem;

using ItemScript;

public class UnitInventoryComponent : MonoBehaviour
{
    public ObjectInventory unitInventory = new ObjectInventory();

    [Header("Equipped Items")]
    public ItemInformation equippedMainHand;
    public ItemInformation equippedOffHand;

    public ItemInformation equippedHelmet, equippedArmor, equippedBoots;

    public void Awake()
    {
        // TODO : Load Items here from Json Data.  ( Create Converter )
        InitializeEquippedToTrue();
        InitializeEquippedToEmpty();
    }

    public void InitializeEquippedToTrue()
    {
        equippedArmor.isEquipped = true;
        equippedArmor.itemType = ItemType.Armor;
        equippedArmor.equippedItem = EquippedItem.DefensiveGear;
        equippedHelmet.isEquipped = true;
        equippedHelmet.itemType = ItemType.Helmet;
        equippedHelmet.equippedItem = EquippedItem.DefensiveGear;
        equippedBoots.isEquipped = true;
        equippedBoots.itemType = ItemType.Helmet;
        equippedBoots.equippedItem = EquippedItem.DefensiveGear;
        equippedOffHand.isEquipped = true;
        equippedMainHand.isEquipped = true;
    }
    public void InitializeEquippedToEmpty()
    {
        equippedMainHand.equippedItem = EquippedItem.Hand;
        equippedOffHand.equippedItem = EquippedItem.Hand;
    }
}
