using System.Linq;
using System.Collections;
using System.Collections.Generic;
using InteractableScripts.Behavior;
using UnityEngine;


using UnitsScripts.Behaviour;
using PlayerScripts.UnitCommands;
using UnitStats;
using ItemScript;

namespace WorldObjectScripts.Behavior
{
    public class Trees : WorldObjectBaseBehaviour
    {
        public float dmgInterval_B = 5.0f;
        // 1
        public override void StartInteraction(InteractingComponent newUnit, ActionType actionIndex)
        {
            if (!canInteract)
            {
                    return;
            }
            UnitBaseBehaviourComponent unit = newUnit.GetComponent<UnitBaseBehaviourComponent>();
            // Obtain Interacting Unit
            if (unit.objectType == ObjectType.PlayerControlled || unit.objectType == ObjectType.Unit)
            {
                Debug.Log("Adding : " + newUnit.transform.name);
                interactingUnit.Add(unit);
            }
            UnitGatheringResourceStats tmp = new UnitGatheringResourceStats();
            InitializeGathererStats(unit, tmp);
            // Get the Physical Damage
            ReceiveDamage(unit, unit.myStats.GetCurrentStats[Stats.Strength].GetLevel);
            
        }
        public override void EndIndividualInteraction(UnitBaseBehaviourComponent unit)
        {
            if (gathererStats.Contains(gathererStats.Find(x => x.unitSaved == unit)))
            {
                gathererStats.Remove(gathererStats.Find(x => x.unitSaved == unit));
                interactingUnit.Remove(unit);
                if(unit.interactWith == this)
                {
                    unit.ReceiveOrder(UnitOrder.GenerateIdleOrder());
                }
            }
        }

        public override void ReceiveDamage(UnitBaseBehaviourComponent sender, float damageReceived)
        {
            base.ReceiveDamage(sender, damageReceived);

            if (sender != null)
            {
                IncrementInteractingUnitsStats(sender, damageReceived);
            }
        }
        public override void EndAllInteraction()
        {
            List<UnitBaseBehaviourComponent> tmp = interactingUnit;

            foreach (UnitBaseBehaviourComponent item in tmp)
            {
                if(item.currentCommand == Commands.GATHER_RESOURCES)
                {
                    item.ReceiveOrder(UnitOrder.GenerateIdleOrder());
                }
            }

            SpawnReward();
            interactingUnit.Clear();
            gathererStats.Clear();
        }
        // 2
        public bool IsUnitGathering(UnitBaseBehaviourComponent unit)
        {
            if(!interactingUnit.Contains(unit))
            {
                return false;
            }
            return (unit.currentCommand == Commands.GATHER_RESOURCES);
        }
        public void InitializeGathererStats(UnitBaseBehaviourComponent unit,UnitGatheringResourceStats tmp)
        {
            if (!tmp.dataInitialized)
            {
                tmp.SetInterval(unit, dmgInterval_B);
                tmp.unitDamage_C = unit.myStats.GetStats(Stats.Strength).GetLevel;
                tmp.dataInitialized = true;
            }
            gathererStats.Add(tmp);
        }
        public override void SpawnReward()
        {
            base.SpawnReward();
            // Spawn Reward here
            foreach (ItemInformation item in rewardInformation)
            {
                Vector3 spawnPos = NavMeshPositionGenerator.GetInstance.GeneratePositionAround(this.transform.position, 1.45f);
                GameObject tmp = GameObject.Instantiate(itemDrop, spawnPos, Quaternion.identity, null);
                tmp.GetComponent<ItemDrop>().SetRewardInformation(item);
            }
        }
    }
}
