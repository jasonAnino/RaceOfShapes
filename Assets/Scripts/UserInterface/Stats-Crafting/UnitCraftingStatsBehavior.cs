using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnitsScripts.Behaviour;

using PlayerScripts.UnitCommands;
using Utilities;
using ItemScript;
using UnitStats;
namespace UserInterface
{
    public class UnitCraftingStatsBehavior : UIBaseBehavior
    {
        public GameObject craftingStatsHolderPrefab;
        public UnitBaseBehaviourComponent owner;
        public GameObject statsHolder;
        public float movementLength;
        // Scroll Related Functionalities
        public Scrollbar scrollBar;
        public RectTransform contentParent;
        public GridLayoutGroup layoutGroup;
        public int columnCount = 0;

        public void InitializeStats()
        {
            if(craftingStatsHolderPrefab == null)
            {
                return;
            }
            foreach(CraftingStats item in Enum.GetValues(typeof(CraftingStats)))
            {
                GameObject tmp = Instantiate(craftingStatsHolderPrefab, statsHolder.transform);
                CraftingStatsHolder holder = tmp.GetComponent<CraftingStatsHolder>();
                holder.Initialize(item, owner);
            }
        }
    }
}