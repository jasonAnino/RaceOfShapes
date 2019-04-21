using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ComboSystem;
using UnitsScripts.Behaviour;

namespace ItemScript
{
    [Serializable]
    public class BaseLootBehavior
    {
        [SerializeField]private ItemInformation itemDrop = new ItemInformation();
        public ItemInformation GetItemDrop
        {
            get { return itemDrop; }
        }
        [Header("Drop Behavior Info")]
        public float dropChance = 10.0f;   // out of 100% drop chance
        public int dropAmount = 1;
        public bool onlyOneItem = true;
    }

}
