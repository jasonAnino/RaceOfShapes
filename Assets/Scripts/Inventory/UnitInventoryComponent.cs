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

    [Header("Unit Weight")]
    public float weight = 0;
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
        equippedHelmet.isEquipped = true;
        equippedHelmet.itemType = ItemType.Helmet;
        equippedBoots.isEquipped = true;
        equippedBoots.itemType = ItemType.Helmet;
        equippedOffHand.isEquipped = true;
        equippedMainHand.isEquipped = true;
    }
    public void InitializeEquippedToEmpty()
    {
        equippedMainHand.itemType = ItemType.Weapon;
        equippedOffHand.itemType = ItemType.Weapon;
    }
   
}
