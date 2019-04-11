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
    public class UnitStatsBehavior : UIBaseBehavior
    {
        public GameObject statsHolderPrefab;
        public UnitBaseBehaviourComponent owner;
        public LayoutSort statsSorter;
        public GameObject statsHolder;
        public List<StatsHolder> currentStatsList;
        public float movementLength;
        public Scrollbar scrollBar;

        public void InitializeStats()
        {
            if(statsHolderPrefab == null)
            {
                return;
            }
            foreach(Stats item in Enum.GetValues(typeof(Stats)))
            {
                GameObject tmp = Instantiate(statsHolderPrefab, statsSorter.transform);
                statsSorter.items.Add(tmp.transform);
                StatsHolder holder = tmp.GetComponent<StatsHolder>();
                holder.Initialize(item, owner);
                currentStatsList.Add(tmp.GetComponent<StatsHolder>());
            }
            statsSorter.UpdateTransformPositions();

            if(currentStatsList.Count > 4)
            {
                float sizeDifference = (157.0f * currentStatsList.Count) - 700;
                AdjustScrollSize(sizeDifference);
                movementLength = sizeDifference;
            }
        }
        public void AdjustPosition()
        {
            float newPos = movementLength * scrollBar.value;
            statsHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, newPos);
        }
        public void AdjustScrollSize(float sizeDifference)
        {
            scrollBar.size -= sizeDifference / 765;
            scrollBar.value = 0;

        }
    }
}
