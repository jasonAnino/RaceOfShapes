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
        Armor = 2,
        Helmet = 3,
        Boots = 4,
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
    /// <summary>
    /// This is where all basic information is found, from the item's name, weight, type.
    /// </summary>
   [Serializable]
    public class ItemInformation
    {
        public string itemName;
        public string id;
        public bool isStackable;
        public bool isEquipped;
        public float weight;
        public ItemType itemType;
        public EquippedItem equippedItem;
        public int count;
        public float damage_Min, damage_Max;
        public float armor_Min, armor_Max;
    }
    public static class ItemsUtility
    {
        public static string GenerateItemID(string baseName)
        {
            int rand = UnityEngine.Random.Range(3, 99);
            string tmp = baseName + "_00" + rand;
            // GetTime
            return tmp;
        }
    }
}
