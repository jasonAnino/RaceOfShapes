using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using PlayerScripts.UnitCommands;
using UnitsScripts.FSM;
using InteractableScripts.Behavior;
using WorldObjectScripts.Behavior;
using Utilities;
using UnitStats;
using ComboSystem;
using SkillBehaviour;
using ItemScript;

namespace UnitsScripts.Behaviour
{
    public enum CreatureBehaviorState
    {
        idle = 0,
        roaming = 1,
        hunting = 2,
        sleeping = 3,
    }
    public class MonsterBaseBehavior : UnitBaseBehaviourComponent
    {
        // Monster Stats
        [Header("Monster Information")]
        public GameObject itemDropPrefab;
        [Header("Monster Drops")]
        public List<BaseLootBehavior> potentialDrops = new List<BaseLootBehavior>();
        
        public override void OnDeath()
        {
            base.OnDeath();
            FilterDroppedItem();
        }

        public void FilterDroppedItem()
        {
            float rng = UnityEngine.Random.Range(0, 99);
            for (int i = 0; i < potentialDrops.Count; i++)
            {
                if(rng < potentialDrops[i].dropChance)
                {
                    SpawnItemDrop(potentialDrops[i].GetItemDrop);
                }
            }
        }
        public void SpawnItemDrop(ItemInformation itemsNewInfo)
        {
            GameObject tmp = Instantiate(itemDropPrefab, null);
            tmp.transform.position = this.transform.position;
            tmp.GetComponent<ItemDrop>().SetRewardInformation(itemsNewInfo);
            Destroy(this.gameObject);
        }
    }
}
