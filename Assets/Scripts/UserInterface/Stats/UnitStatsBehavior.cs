using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnitsScripts.Behaviour;

using PlayerScripts.UnitCommands;
using Utilities;
using ItemScript;
using UnitStats;
namespace UserInterface
{
    public class UnitStatsBehavior : MonoBehaviour
    {
        public GameObject statsHolderPrefab;
        public UnitBaseBehaviourComponent owner;
        public LayoutSort statsSorter;
        public List<StatsHolder> currentStatsList;

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
            }
            statsSorter.UpdateTransformPositions();
        }
    }
}
