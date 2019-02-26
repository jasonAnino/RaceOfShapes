using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InteractableScripts.Behavior;
using ComboSystem;

namespace ItemScript
{
    public enum ItemType
    {
        Weapon = 0,
        Ingredient = 1,
        Potion = 2,
        Armor = 3,
        Helmet = 4,
        Boots = 5,
        Pants = 6,
        Cape = 7,
        Ring = 8,
        Belt = 9,
        Glasses = 10,
        Necklace = 11,
        Gloves = 12,
    }

    /// <summary>
    /// This is the basic 'loot' system, this is where you get information about the loot.
    /// </summary>
    public class BaseItemComponent : InteractingComponent
    {
        public ItemInformation itemInfo;

        public override void Awake()
        {
            base.Awake();
            if(itemInfo.count <= 0)
            {
                itemInfo.count = 1;
            }
            if(itemInfo.isStackable)
            {
                itemInfo.id = ItemsUtility.GenerateItemID(itemInfo.itemName);
            }
        }

        public ItemInformation ObtainInformation()
        {
            return this.itemInfo;
        }

    }
}
