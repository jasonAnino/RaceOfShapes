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
        public GameObject statsHolder;
        public OverallStatsHolder overallStatHolder;
        public float movementLength;
        private CharacterStatsSystem ownerStats;
        // Scroll Related Functionalities
        public Scrollbar scrollBar;
        public RectTransform contentParent;
        public GridLayoutGroup layoutGroup;
        public int columnCount = 0;

        public void InitializeStats()
        {
            if(statsHolderPrefab == null)
            {
                return;
            }
            ownerStats = owner.myStats;

            foreach (Stats item in Enum.GetValues(typeof(Stats)))
            {
                GameObject tmp = Instantiate(statsHolderPrefab, statsHolder.transform);
                StatsHolder holder = tmp.GetComponent<StatsHolder>();
                holder.overAll = overallStatHolder;
                holder.Initialize(item, owner);
            }
            GetColumnCount();
            for (int i = 0; i < Enum.GetValues(typeof(NumericalStats)).Length; i++)
            {
                NumericalStats tmp = (NumericalStats)i;
                Debug.Log("Initializing :" + tmp + " With Amount : " + ownerStats.GetUnitNumericalStats[tmp].currentCount);
                overallStatHolder.AddHolderCount((int)tmp, ownerStats.GetUnitNumericalStats[tmp].currentCount);
            }
        }

        #region SCROLLBAR_RELATED_FUNCTIONS
        public void AdjustPosition()
        {
            float newPos = movementLength * scrollBar.value;
            statsHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, newPos);
        }
        public void AdjustScrollBarSize()
        {
            if(columnCount >= 3)
            {
                scrollBar.size = 1.0f - (((130 * columnCount) / 292.5f)  - 1.0f); 
            }
        }
        #endregion

        #region LAYOUTGROUP_RELATED_FUNCTIONS
        public void GetColumnCount()
        {
            float columnXmark = layoutGroup.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x;
            for(int x = 0; x < layoutGroup.transform.childCount; x++)
            {
                if(columnXmark == layoutGroup.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x)
                {
                    columnCount++;
                }
            }
        }
        #endregion
    }
}
