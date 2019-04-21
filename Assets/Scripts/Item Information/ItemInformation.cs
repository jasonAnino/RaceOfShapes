using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ComboSystem;
using UnitsScripts.Behaviour;
using UnitStats;
namespace ItemScript
{
    [Serializable]
    /// <summary>
    /// This is where all basic information is found, from the item's name, weight, type.
    /// </summary>
    public class ItemInformation
    {
        [Header("ITEM INFORMATION")]
        public string itemName;
        public string id;
        public bool isStackable;
        public bool isEquipped;
        public bool isConsumable;
        public float weight;
        public int count;  // amount of items in the slot
        public ItemType itemType;

        [Header("TARGET INFORMATION")]
        public TargetType inflictBuffTo;  // if used as ingredient, Self : self, SingleTarget : Aim, MultipleTarget : whole party, Aoe : Aura, Wave : Whole Area.
        public InflictionType damageOrHeal; // if used as ingredient, Offensive : Negative buff, Defensive : Positive Buff.
        public TargetStats targetStats; // if used as ingredient, + to stats (offensive) / - to stats (defensive).
        public float min, max;  // if used as ingredient, lets use min/max as data to how the stats would adjust.

        [Header("POTION INFORMATION")]
        public float duration = 120.0f;

        public static ItemInformation DeepCopy(ItemInformation copyThis)
        {
            ItemInformation tmp = new ItemInformation();
            tmp.itemName = copyThis.itemName;
            tmp.id = copyThis.id;
            tmp.isStackable = copyThis.isStackable;
            tmp.isEquipped = copyThis.isEquipped;
            tmp.isConsumable = copyThis.isConsumable;
            tmp.weight = copyThis.weight;
            tmp.count = copyThis.count;
            tmp.itemType = copyThis.itemType;

            tmp.inflictBuffTo = copyThis.inflictBuffTo;
            tmp.damageOrHeal = copyThis.damageOrHeal;
            tmp.targetStats = copyThis.targetStats;
            tmp.min = copyThis.min;
            tmp.max = copyThis.max;

            return tmp;
        }


        public void UseItem(UnitBaseBehaviourComponent thisPlayer)
        {
            switch(itemType)
            {
                case ItemType.Belt:
                case ItemType.Boots:
                case ItemType.Cape:
                case ItemType.Glasses:
                case ItemType.Gloves:
                case ItemType.Helmet:
                case ItemType.Necklace:
                case ItemType.Pants:
                case ItemType.Ring:
                case ItemType.Weapon:
                case ItemType.Armor:

                    break;
                case ItemType.Potion:
                    if(isConsumable)
                    {
                        UsePotion(thisPlayer);
                    }

                    break;
            }
        }
        public void UsePotion(UnitBaseBehaviourComponent thisPlayer)
        {
            float AddOrSubtractMin = 0;
            float AddOrSubtractMax = 0;
            switch(damageOrHeal)
            {
                case InflictionType.Defensive:
                    AddOrSubtractMin += min;
                    AddOrSubtractMax += max;
                    break;
                case InflictionType.Offensive:
                    AddOrSubtractMin -= min;
                    AddOrSubtractMax -= max;
                    break;
            }

            switch(targetStats)
            {
                case TargetStats.curHealth:
                    thisPlayer.ReceiveHeal(min, NumericalStats.Health);
                    break;
                case TargetStats.maxHealth:
                    thisPlayer.myStats.GetUnitNumericalStats[NumericalStats.Health].AddedToTemporaryCounts(TemporaryCount.CreateTmpCount(max, duration));
                    break;
                case TargetStats.curMana:
                    thisPlayer.myStats.GetUnitNumericalStats[NumericalStats.Mana].currentCount += min;
                    break;
                case TargetStats.maxMana:
                    thisPlayer.myStats.GetUnitNumericalStats[NumericalStats.Health].AddedToTemporaryCounts(TemporaryCount.CreateTmpCount(max, duration));
                    break;
                case TargetStats.statsAgi:
                    thisPlayer.myStats.GetUnitNumericalStats[NumericalStats.Speed].currentCount += min;
                    break;
            }
        }
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